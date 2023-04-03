using System;
namespace ExperisVinlotteri.Services
{
	public interface IRandomService
	{
        int Next(int minValue, int maxExclusiveValue);

    }
}

