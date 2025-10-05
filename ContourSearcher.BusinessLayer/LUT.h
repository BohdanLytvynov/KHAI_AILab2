#ifndef LUT_H

#define LUT_H

template<class TPixel>
struct LUTBase
{
	LUTBase()
	{
		m_vector(256, 0);
	}

private:
	std::vector<TPixel> m_vector;
};



struct LUTFactory
{

};

#endif


