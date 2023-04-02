using System;
namespace ExperisVinlotteri.Models
{
	public class ListOfUsers
	{
		public ListOfUsers(IEnumerable<UserDto> initContent)
		{
			Users = initContent.ToList();

        }

		public List<UserDto> Users = new List<UserDto>();
	}
}




