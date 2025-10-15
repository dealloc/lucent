# Lucent

A lean, minimalistic .NET library that provides idiomatic integration of Lucene.NET with dependency injection and modern .NET configuration patterns.

## Overview

Lucent is designed to bring Lucene.NET closer to idiomatic .NET best practices while maintaining direct access to the underlying Lucene.NET types and APIs. Unlike heavier abstraction layers, Lucent focuses on providing just what you need: service container integration, configuration management, and lifecycle handling.

## Key Features

- **Minimal Abstraction**: Direct access to Lucene.NET types and APIs
- **Service Container Integration**: Native support for .NET dependency injection
- **Idiomatic Configuration**: Leverage `IConfiguration` and options patterns
- **Lifecycle Management**: Proper disposal and resource management
- **Lean Design**: No unnecessary wrappers or query builders

## Philosophy

Lucent takes a different approach compared to libraries like Examine. While Examine provides extensive abstractions and query builders, Lucent maintains the full power and flexibility of Lucene.NET while simply making it easier to use in modern .NET applications.

If you require more high-level abstractions, fluent query builders, we recommend checking out the excellent [Examine](https://github.com/Shazwazza/Examine) library.

## Comparison with Examine

| Feature | Lucent | Examine |
|---------|---------|---------|
| Abstraction Level | Minimal | High |
| Query Building | Direct Lucene.NET | Fluent API |
| Learning Curve | Requires Lucene.NET knowledge | Examine-specific |
| Flexibility | Full Lucene.NET power | Limited to abstractions |
| Use Case | Lucene users | Quick implementation |

## Requirements

- .NET 9.0 or later
- Lucene.NET 4.8-beta00017 or later

## Contributing

Contributions are welcome! Please read our [contributing guidelines](CONTRIBUTING.md) and submit pull requests to the `develop` branch.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

- Built on top of the excellent [Lucene.NET](https://lucenenet.apache.org/) library
- Inspired by the simplicity needs not met by existing abstraction layers