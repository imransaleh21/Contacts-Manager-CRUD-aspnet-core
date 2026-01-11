# Security Policy

## 🔒 Supported Versions

We release patches for security vulnerabilities. Currently supported versions:

| Version | Supported          |
| ------- | ------------------ |
| 1.0.x   | :white_check_mark: |
| < 1.0   | :x:                |

## 🔍 Reporting a Vulnerability

We take the security of Contacts Manager seriously. If you believe you have found a security vulnerability, please report it to us responsibly.

### 📧 How to Report

**Please do NOT report security vulnerabilities through public GitHub issues.**

Instead, please report them via one of the following methods:

1. **GitHub Security Advisories** (Preferred)
   - Navigate to the [Security tab](https://github.com/imransaleh21/Contacts-Manager-CRUD-aspnet-core/security) of this repository
   - Click "Report a vulnerability"
   - Fill out the form with details

2. **Direct Contact**
   - Contact the maintainer directly through GitHub: [@imransaleh21](https://github.com/imransaleh21)
   - Include "SECURITY" in the subject line

### 📝 What to Include

Please include the following information in your report:

- Type of vulnerability
- Full paths of source file(s) related to the vulnerability
- Location of the affected source code (tag/branch/commit or direct URL)
- Step-by-step instructions to reproduce the issue
- Proof-of-concept or exploit code (if possible)
- Impact of the vulnerability, including how an attacker might exploit it

### ⏱️ Response Timeline

- **Initial Response**: Within 48 hours of report submission
- **Status Update**: Within 7 days with our evaluation and expected timeline
- **Resolution**: We'll work to patch verified vulnerabilities as quickly as possible

### 🎯 What to Expect

1. **Acknowledgment**: We'll acknowledge your report within 48 hours
2. **Investigation**: We'll investigate and validate the vulnerability
3. **Fix Development**: If confirmed, we'll develop a fix
4. **Disclosure**: We'll coordinate with you on public disclosure timing
5. **Credit**: We'll credit you in the security advisory (unless you prefer to remain anonymous)

## 🛡️ Security Best Practices

When using this application, please follow these security best practices:

### For Developers

- Keep all dependencies up to date
- Use strong connection strings and don't commit secrets
- Enable HTTPS in production
- Implement proper input validation
- Use parameterized queries (already implemented via EF Core)
- Follow OWASP Top 10 guidelines

### For Deployment

- Use secure SQL Server configurations
- Implement proper authentication and authorization
- Use environment variables for sensitive configuration
- Enable CORS appropriately
- Use HTTPS in production
- Implement rate limiting
- Regular security audits and updates

## 🔐 Security Features

This application includes:

- ✅ Input validation through model validation
- ✅ SQL injection prevention via Entity Framework Core
- ✅ XSS protection through Razor encoding
- ✅ CSRF protection via ASP.NET Core anti-forgery tokens
- ✅ Secure connection strings configuration
- ✅ Custom validation attributes

## 📚 Resources

- [OWASP Top 10](https://owasp.org/www-project-top-ten/)
- [ASP.NET Core Security](https://docs.microsoft.com/en-us/aspnet/core/security/)
- [Entity Framework Core Security](https://docs.microsoft.com/en-us/ef/core/miscellaneous/security)

## 🙏 Thank You

We appreciate your efforts to responsibly disclose your findings and will make every effort to acknowledge your contributions.

---

**Last Updated**: 2024-01-11
