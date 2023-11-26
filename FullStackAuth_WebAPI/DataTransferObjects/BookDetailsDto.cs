using System;
namespace FullStackAuth_WebAPI.DataTransferObjects
{
	public class BookDetailsDto
	{
		public List<ReviewWithUserDto> Reviews { get; set; }
		public double AverageRating { get; set; }
	}
}

