# advisory-workflow-dashboard

Central advisor/wealth strategist workspace that serves as both:
- the main dashboard application, and
- the index repo for the broader advisory tool suite (loose integration model).

## Platform direction
- Internal desktop browser first
- Module-tile navigation
- Mock data for v1
- Strong module registry for internal modules and external GitHub repos

## Current modules
- **Client Meeting Prep** (internal module, MVP)
  - Draft/review/approve packet workflow
  - Excel/PowerPoint/Outlook draft actions
  - Audit timeline

## Registry and integration model
- Module metadata source: `src/api/module-registry/modules.json`
- Registry API: `GET /api/module-registry`
- UI renders module tiles from registry at runtime.
- Internal modules open local routes; external modules open linked repos.

See `docs/module-registry.md` for onboarding rules.

## Run
1. `dotnet restore client-meeting-prep-office-suite.slnx`
2. `dotnet run --project src/api/ClientMeetingPrep.Api.csproj`
3. Open:
   - Dashboard hub: `/`
   - Meeting Prep module: `/modules/meeting-prep.html`

## Sample workflow today
1. Open dashboard and launch **Client Meeting Prep**.
2. Click **Load Clients**.
3. Click **Quickstart Packet**.
4. Run exports and load audit events.

## Planned external repos (initial links)
- `advisor-proposal-scenario-builder`
- `advisor-tax-withdrawal-planner`
- `advisor-ips-compliance-checker`

## Next hardening
- Add Entra ID auth and role-based access
- Replace mock adapters with real CRM/portfolio integrations
- Implement production Office file generation + document storage
- Add environment-specific module registry (dev/test/prod)
