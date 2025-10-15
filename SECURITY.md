# Security Policy

## Supported Versions

We provide security updates for the following versions of Lucent:

| Version | Supported          |
| ------- | ------------------ |
| Latest  | :white_check_mark: |
| < Latest| :x:               |

As Lucent is currently in early development, we only support the latest released version with security updates.

## Reporting a Vulnerability

If you discover a security vulnerability in Lucent, please report it responsibly by following these steps:

### How to Report

1. **Do not** open a public issue on GitHub
2. Report through the security tab
3. Include as much information as possible:
   - Description of the vulnerability
   - Steps to reproduce
   - Potential impact
   - Suggested fix (if available)

### What to Expect

- **Acknowledgment**: We will acknowledge receipt of your report within 48 hours
- **Assessment**: We will assess the vulnerability and determine its severity
- **Timeline**: We aim to provide a timeline for resolution within 7 days
- **Updates**: We will keep you informed of our progress
- **Credit**: With your permission, we will credit you in our security advisory

### Response Timeline

- **Critical vulnerabilities**: Patched within 7 days
- **High severity**: Patched within 14 days
- **Medium/Low severity**: Patched in the next regular release cycle

## Security Considerations

### Index Security

When using Lucent, be aware that:

- **Index files** should be stored securely with appropriate file system permissions
- **Sensitive data** should be carefully considered before indexing
- **Access controls** should be implemented at the application level

### Dependencies

Lucent relies on:
- Lucene.NET - We monitor security advisories for the underlying Lucene.NET library
- .NET Runtime - Ensure you're using supported .NET versions with latest security updates

### Best Practices

When using Lucent in production:

1. **Validate input** before indexing to prevent injection attacks
2. **Sanitize queries** from user input
3. **Implement proper authentication** and authorization
4. **Monitor access** to index files and directories
5. **Regular updates** - Keep Lucent and its dependencies updated
6. **Secure configuration** - Protect configuration files containing sensitive paths

## Disclosure Policy

- We will coordinate public disclosure of vulnerabilities
- Security advisories will be published on GitHub
- We follow responsible disclosure practices
- Public disclosure typically occurs after fixes are available

## Security Contact

For security-related inquiries that are not vulnerabilities (questions about best practices, security features, etc.), you can:
- Open a public issue on GitHub with the "security" label
- Start a discussion in the repository

Thank you for helping keep Lucent secure!