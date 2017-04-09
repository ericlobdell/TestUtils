using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace TestUtils.Tests
{
  public class RequestDataBuilderTests
  {
    [Fact]
    public void Set_sets_property_value()
    {
      var sut = new RequestDataBuilder<Person>()
        .Set(p => p.Name, "Eric")
        .Set(p => p.Age, 12)
        .Create();

      Assert.True(sut.ContainsKey("Name"));
      Assert.True(sut.ContainsKey("Age"));
    }

    [Fact]
    public void Default_ctor_sets_no_values()
    {
      var sut = new RequestDataBuilder<Person>()
        .Create();

      Assert.True(sut.Count == 0);
    }

    [Theory, AutoData]
    public void Template_ctor_maps_property_values_from_template(Person template)
    {
      var sut = new RequestDataBuilder<Person>(template)
        .Create();

      Assert.Equal(sut["Name"], template.Name);
      Assert.Equal(sut["Age"], template.Age);
    }

    [Theory, AutoData]
    public void Set_overwrites_property_values_from_template(Person template)
    {
      var sut = new RequestDataBuilder<Person>()
        .Set(p => p.Name, "Eric")
        .Create();

      Assert.NotEqual(sut["Name"], template.Name);
    }

    [Theory, AutoData]
    public void Omit_removes_properties(Person template)
    {
      var sut = new RequestDataBuilder<Person>(template)
        .Omit(p => p.Age)
        .Create();

      Assert.False(sut.ContainsKey("Age"));
    }
  }

  public class Person
  {
    public string Name { get; set; }
    public int Age { get; set; }
  }
}
