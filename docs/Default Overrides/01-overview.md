# Overview of using defaults

When using `AddDumpCapturingHandlerWithDefaults` a number of optional configuration parameters are available to further configure the default.

The sections here outline what each one does and any caveats around their use.

## The `configurator` parameter

This action is run *before* the defaults are added and allows for adding custom options via the `IDumpCapturerBuilder` interface and its associated extension methods.

The following example shows the adding of a content type deserialiser for `HTML` content using a custom content type deserialiser:

```csharp  { data-fiddle="x9v078" }
services.AddHttpClient<MyTestClient>()            
    .AddRequestAndResponseCapturing(c => c
        .AddDumpCapturingHandlerWithDefaults(
            configurator: c => c
                .AddContentTypeBasedDeserialiser<HtmlDeserialiser>()
        )
    );
```

!!! info
    Because this is added before all the defaults it allows the `HtmlDeserialiser` to run before the default `TextSerialiser`
    which matches `text/*` content types.

!!! warning
    This content type deserialiser is not part of the library and is only shown for illustrative purposes of execution order.

    The `DotNetFiddle` includes the implementation and will pretty-print the `HTML`.

    Under the hood, this deserialiser uses [AngleSharp](https://www.nuget.org/packages/AngleSharp) to prettify the `HTML` content.

## The `defaultDumpHandlerConfigurator` parameter

This action will configure the `DefaultDumpHandler`'s options which allows for configuration of output folders, file name providers
and the `JsonSerializerOptions` that are used to output the `JSON` request and response dump to a file.

The following example shows the adding of a content type deserialiser for `HTML` content using a custom content type deserialiser:

!!! warning
    This example intentionally uses the same filename for all requests. This should **NOT** be done in real scenarios
    as multiple threads may attempt to write to the same file, resulting in failure.

```csharp  { data-fiddle="kb8FSA" }
services
    .AddHttpClient<MyTestClient>()
    .AddRequestAndResponseCapturing(c => c
        .AddDumpCapturingHandlerWithDefaults(
            defaultDumpHandlerConfigurator: c => c.UseFilenameResolverDelegate(dump => "my file")
        )
    )
```
## The `defaultHeaderMapperConfigurator` parameter

This action will configure the default header mapper's options. 

## The `queryParameterObfuscatingUriTransformerConfigurator` parameter

This action allows for further configuration of the query parameter obfuscating `IUriTransformer`