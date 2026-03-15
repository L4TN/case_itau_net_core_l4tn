using AutoMapper;
using CaseItau.Application.Mappings;
using FluentAssertions;
using Xunit;

namespace CaseItau.Tests.Mappings;

public class MappingProfileTests
{
    [Fact]
    public void MappingProfile_DeveSerValido()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        config.AssertConfigurationIsValid();
    }
}
