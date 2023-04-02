using System;


namespace ExperisVinlotteri.Models
{
    public class UserDto
    {
        public UserDto(string id,string name, int selectedNumber)
        {
            Id = id;
            Name = name;
            SelectedNumber = selectedNumber;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public int SelectedNumber { get; set; }

    }
}

