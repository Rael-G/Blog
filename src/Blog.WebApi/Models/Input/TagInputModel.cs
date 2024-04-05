﻿using System.ComponentModel.DataAnnotations;
using Blog.Application;

namespace Blog.WebApi;

public record TagInputModel : IInputModel<TagDto>
{
    [Required]
    public string Name { get; set; }

    public TagInputModel(string name)
    {
        Name = name;
    }

    public TagDto InputToDto()
        => new(Guid.NewGuid(), Name, []);

    public void InputToDto(TagDto tagDto)
    {
        tagDto.Name = Name;
    }
}
