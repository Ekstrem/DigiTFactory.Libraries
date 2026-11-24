using DigiTFactory.Libraries.SeedWorks.Serialization;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using Xunit;

namespace DigiTFactory.Libraries.ValueObjectSerialization.Tests;

public sealed class ValueObjectSerializerTests
{
    public static IEnumerable<object[]> Formats => new[]
    {
        new object[] { ValueObjectSerializationFormat.JsonEnvelope },
        new object[] { ValueObjectSerializationFormat.Protobuf },
        new object[] { ValueObjectSerializationFormat.MessagePack },
    };

    [Theory]
    [MemberData(nameof(Formats))]
    public void RoundTrip_WhenMultipleVos_ShouldPreserveValues(ValueObjectSerializationFormat format)
    {
        var serializer = ValueObjectSerializerFactory.Create(format, Registry());
        var source = new Dictionary<string, IValueObject>
        {
            ["Name"] = new StringVo("hello"),
            ["Count"] = new IntVo(7),
            ["Flag"] = new BoolVo(true),
        };

        var round = serializer.Deserialize(serializer.Serialize(source));

        Assert.Equal(3, round.Count);
        Assert.Equal("hello", Assert.IsType<StringVo>(round["Name"]).Value);
        Assert.Equal(7, Assert.IsType<IntVo>(round["Count"]).Value);
        Assert.True(Assert.IsType<BoolVo>(round["Flag"]).Value);
    }

    [Theory]
    [MemberData(nameof(Formats))]
    public void RoundTrip_WhenEmptyDictionary_ShouldReturnEmpty(ValueObjectSerializationFormat format)
    {
        var serializer = ValueObjectSerializerFactory.Create(format, Registry());
        var round = serializer.Deserialize(serializer.Serialize(new Dictionary<string, IValueObject>()));
        Assert.Empty(round);
    }

    [Fact]
    public void CreateDefault_ShouldReturnJsonEnvelope()
    {
        var serializer = ValueObjectSerializerFactory.CreateDefault(Registry());
        Assert.IsType<JsonEnvelopeValueObjectSerializer>(serializer);
    }

    [Fact]
    public void Registry_WhenUnknownType_ShouldThrow()
    {
        var registry = new ValueObjectTypeRegistry();
        Assert.Throws<System.InvalidOperationException>(() => registry.ResolveType("No.Such.Type"));
    }

    private static IValueObjectTypeRegistry Registry()
        => new ValueObjectTypeRegistry()
            .Register<StringVo>("str")
            .Register<IntVo>("int")
            .Register<BoolVo>("bool");
}

public sealed record StringVo(string Value) : IValueObject;
public sealed record IntVo(int Value) : IValueObject;
public sealed record BoolVo(bool Value) : IValueObject;
