# Git Flow Branch Name Validation

This repository now includes automatic branch name validation for Pull Requests to enforce Git Flow naming conventions.

## Branch Naming Rules

All Pull Request branches must follow one of these patterns:

- `feature/description` - For new features
- `release/version` - For release preparations  
- `hotfix/description` - For urgent bug fixes

## Examples

✅ **Valid branch names:**
- `feature/add-user-authentication`
- `feature/implement-payment-gateway`
- `release/v1.0.0`
- `release/v2.1.0-beta`
- `hotfix/critical-security-fix`
- `hotfix/database-connection-issue`

❌ **Invalid branch names:**
- `main`
- `develop`
- `bugfix/some-issue`
- `copilot/fix-something`
- `feature/` (empty description)

## How It Works

The validation is performed by a GitHub Actions workflow (`.github/workflows/branch-name-check.yml`) that:

1. Triggers automatically on Pull Request events (opened, synchronized, reopened)
2. Checks if the branch name matches the required pattern
3. Provides clear feedback when validation fails
4. Blocks the PR if the branch name doesn't follow conventions

## Workflow Details

- **Trigger Events:** `pull_request` with types `[opened, synchronize, reopened]`
- **Runner:** `ubuntu-latest`
- **Validation Pattern:** `^(feature|release|hotfix)\/.+`

If your branch name doesn't follow the convention, the workflow will fail with helpful examples of correct naming patterns.