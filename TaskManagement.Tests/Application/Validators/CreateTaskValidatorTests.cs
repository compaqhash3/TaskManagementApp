using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Validators;

namespace TaskManagement.Tests.Application.Validators
{
    public class CreateTaskValidatorTests
    {
        private readonly CreateTaskValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_Title_Is_Empty()
        {
            var dto = new CreateTaskDto { Title = "" };
            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.Title);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Title_Is_Valid()
        {
            var dto = new CreateTaskDto { Title = "Valid Title" };
            var result = _validator.TestValidate(dto);
            result.ShouldNotHaveValidationErrorFor(x => x.Title);
        }
    }
}
