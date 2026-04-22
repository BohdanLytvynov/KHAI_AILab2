import os
import numpy as np
import seaborn as sns
import matplotlib.pyplot as plt
from collections import Counter

# Kaggle dataset helper
import kagglehub

# TensorFlow / Keras
import tensorflow as tf
from tensorflow.keras import layers
from tensorflow.keras.models import Sequential
from tensorflow.keras.preprocessing import image_dataset_from_directory
from tensorflow.keras.applications.resnet50 import ResNet50
from tensorflow.keras.applications.resnet50 import preprocess_input

# Metrics
from sklearn.metrics import classification_report
from sklearn.metrics import confusion_matrix

# Ignore warnings
import warnings
warnings.filterwarnings("ignore")
# Suppress TensorFlow logs
os.environ['TF_CPP_MIN_LOG_LEVEL'] = '3' #hide all except the Fatal errors
import tensorflow as tf
import tf2onnx
import onnx

# Download the dataset from KaggleHub If that was already downloaded this command will be ignored
path = kagglehub.dataset_download("sponishflea/classification-of-skin-diseases")

print("\nPath to dataset files:", path)

# Training directory
data_dir=os.path.join(path,'train')

# Image settings
image_size=(224,224) #Standart image dimension
batch_size=16 #Learning batch size, we use 16 images each time

# Split into training and validation
train_ds=image_dataset_from_directory(
    data_dir,
    image_size=image_size,
    batch_size=batch_size,
    subset='training',
    validation_split=0.2,
    seed=46,
    label_mode='int'
)
val_ds=image_dataset_from_directory(
    data_dir,
    image_size=image_size,
    batch_size=batch_size,
    subset='validation',
    validation_split=0.2,
    seed=46,
    label_mode='int'
)

# Check Class Names
class_names=train_ds.class_names
# Prepare test data
test_size=int(len(train_ds)*0.2)
test_ds=train_ds.take(test_size)
train_ds=train_ds.skip(test_size)

print(f"\nsize of training dataset: { len(train_ds)}")
print(f"\nsize of validation dataset: { len(val_ds)}")
print(f"\nsize of test dataset: { len(test_ds)}")

class_counts = Counter()

for images, labels in train_ds:
    class_counts.update(labels.numpy())

print("\nClass distribution:", class_counts)

for i in class_counts:
  print(f"\t{class_names[i]} : {class_counts[i]}")

plt.figure(figsize=(8,5))
bars = plt.bar(class_names, [class_counts[i] for i in range(len(class_names))], color='skyblue')
plt.title("Class Distribution in Training Set")
plt.xlabel("Classes")
plt.ylabel("Number of Images")

# Add counts on top of bars
for bar, count in zip(bars, [class_counts[i] for i in range(len(class_names))]):
    plt.text(bar.get_x() + bar.get_width()/2, bar.get_height() + 2, str(count), ha='center', va='bottom')

plt.show()

### 9. Display Sample Images
plt.figure(figsize=(14,10))

for images, labels in train_ds.take(1):
    for i in range(9):
        plt.subplot(3,3,i+1)
        plt.imshow(images[i].numpy().astype('uint8'))
        plt.title(class_names[labels[i].numpy()])
        plt.axis('off')

plt.show()

#Load Pre-trained ResNet50 Model
base_model = ResNet50(
    include_top=False,
    weights='imagenet',
    input_shape=(224, 224, 3)
)

for layer in base_model.layers[:140]:
    layer.trainable = False
for layer in base_model.layers[140:]:
    layer.trainable = True

#Data Augmentation
data_augmentation = tf.keras.Sequential([
    layers.RandomFlip("horizontal"),
    layers.RandomRotation(0.1),
    layers.RandomZoom(0.1),
])

train_ds = train_ds.map(
    lambda x, y: (data_augmentation(x, training=True), y)
)

#Optimize Dataset Performance
train_ds = train_ds.cache().prefetch(buffer_size=tf.data.AUTOTUNE)
val_ds = val_ds.cache().prefetch(buffer_size=tf.data.AUTOTUNE)

#Define Normalization Layer
normalization = layers.Lambda(preprocess_input)

#Build CNN Model
model = Sequential([
    normalization,
    base_model,
    layers.GlobalAveragePooling2D(),

    # Fully connected layers with more Dropout
    layers.Dense(64, activation='relu'),
    layers.Dropout(0.3),      # increased dropout

    layers.Dense(128, activation='relu'),
    layers.Dropout(0.4),

    layers.Dense(256, activation='relu'),
    layers.Dropout(0.4),

    layers.Dense(128, activation='relu'),  # added one more layer
    layers.Dropout(0.3),

    layers.Dense(len(class_names), activation='softmax')
])

model.compile(
    optimizer=tf.keras.optimizers.Adam(learning_rate=1e-3),
    loss='sparse_categorical_crossentropy',
    metrics=['accuracy']
)

#Define Callbacks
early_stop = tf.keras.callbacks.EarlyStopping(
    monitor='val_loss',
    patience=5,
    restore_best_weights=True
)

lr_scheduler = tf.keras.callbacks.ReduceLROnPlateau(
    monitor='val_loss',
    factor=0.3,
    patience=3,
    min_lr=1e-6,
    verbose=1
)

#Train model
history = model.fit(
    train_ds,
    epochs=30,
    validation_data=val_ds,
    callbacks=[early_stop, lr_scheduler]
)

#Plot Training History
print("\nDisplay Training History Results")
plt.figure(figsize=(12,4))

plt.subplot(1,2,1)
plt.plot(history.history['accuracy'], label='train acc')
plt.plot(history.history['val_accuracy'], label='val acc')
plt.title('Accuracy')
plt.legend()

plt.subplot(1,2,2)
plt.plot(history.history['loss'], label='train loss')
plt.plot(history.history['val_loss'], label='val loss')
plt.title('Loss')
plt.legend()

plt.show()

#Evaluate on Test Dataset
test_ds = test_ds.cache().prefetch(tf.data.AUTOTUNE)

test_loss, test_acc = model.evaluate(test_ds)
print("Test accuracy:", test_acc)

#Classification Report
y_true = []
y_pred = []

for images, labels in test_ds:
    predictions = model.predict(images,verbose=0)
    predicted = np.argmax(predictions, axis=1)

    y_true.extend(labels.numpy())
    y_pred.extend(predicted)

cm=classification_report(y_true,y_pred)
print(cm)

#Confusion Matrix
print("\nDisplay Confusion Matrix.")
cm_matrix = confusion_matrix(y_true, y_pred)

plt.figure(figsize=(10,5))
sns.heatmap(cm_matrix, annot=True, fmt='d', cmap='Blues',
            xticklabels=class_names, yticklabels=class_names)
plt.xlabel('Predicted')
plt.ylabel('True')
plt.title('Confusion Matrix')
plt.show()

#Display Sample Test Predictions
print("\nDisplay Sample Test Predictions.")
plt.figure(figsize=(14, 10))
for images, labels in test_ds.take(1):
    predictions = model.predict(images, verbose=0)
    predicted_labels = np.argmax(predictions, axis=1)

    for i in range(9):  # Display first 9 images
        plt.subplot(3, 3, i+1)
        plt.imshow(images[i].numpy().astype('uint8'))

        true_label = class_names[labels[i].numpy()]
        pred_label = class_names[predicted_labels[i]]

        color = 'green' if predicted_labels[i] == labels[i].numpy() else 'red'
        plt.title(f"True: {true_label}\nPred: {pred_label}", color=color)

        plt.axis('off')
plt.show()

# Entrance specification
spec = (tf.TensorSpec((None, 224, 224, 3), tf.float32, name="input"),)

# Convertion and saving
# 1. Export the TensorFlow model
export_path = "exported_model"
model.export(export_path) 

# 2. Convert to ONNX
print("\nStarting convertion to ONNX...")
os.system(f"python -m tf2onnx.convert --saved-model {export_path} --output disease_scanner.onnx --opset 13")

if os.path.exists("disease_scanner.onnx"):
    print("\nModel ready for usage in a C++.")
    print(f"Path to file: {os.path.abspath('disease_scanner.onnx')}")
    print("Copy it to APP.CONFIG:", ",".join(train_ds.class_names))
else:
    print("\nError!")



