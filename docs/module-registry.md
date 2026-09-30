# Module Registry Governance

The advisory workflow dashboard uses a loose-integration module registry to surface both internal modules and external tool repos.

## Source of truth
- File: `src/api/module-registry/modules.json`
- API: `GET /api/module-registry`
- UI: root dashboard (`/`) renders module tiles from the API response

## Schema fields
- `id`: stable technical key
- `name`: tile display name
- `category`: business grouping (Planning, Compliance, Workflow, etc.)
- `status`: lifecycle marker (`MVP`, `Planned`, `Pilot`, `Production`)
- `integrationType`: `internal` or `external`
- `route`: internal module route (required when `integrationType=internal`)
- `repositoryUrl`: GitHub repo URL (required for all modules)
- `description`: concise tile description
- `sortOrder`: integer display ordering
- `isEnabled`: toggle visibility/action readiness

## Onboarding a new module
1. Add/confirm module repo on GitHub.
2. Add entry in `modules.json`.
3. If internal, create a route/page under `wwwroot/modules` (or future SPA route).
4. Validate tile render and launch behavior from dashboard.
5. Update README planned module list if needed.

## Loose integration rules
- Keep modules independently deployable and versioned.
- Avoid cross-repo runtime coupling where possible.
- Prefer API contracts and web links over code embedding/submodules for v1.
- Keep backward-compatible fields when extending registry schema.
