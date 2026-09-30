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
  - Draft/review/approve packet workflow with Office outputs
- **Proposal Scenario Builder** (external repo, MVP)
  - Compare current vs. proposed client strategy scenarios
  - GitHub: https://github.com/john-stromberg/advisor-proposal-scenario-builder
- **Wealth Planning Calculator** (external repo, MVP)
  - Retirement, FIRE, tax-aware growth, inflation scenarios
  - GitHub: https://github.com/john-stromberg/wealth-planning-calculator

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
1. Open dashboard hub at `/`
2. Click **Client Meeting Prep** to draft/review/approve packets, or click **Proposal Scenario Builder** to model strategy scenarios.
3. For Meeting Prep: Load clients → Quickstart packet → Review exports → Load audit events.
4. For Proposal Builder: Enter client details → Create current/proposed scenarios → Run comparison.

## Planned external repos (next to scaffold)
- `portfolio-risk-lab` - Monte Carlo simulation, drawdown analysis, Sharpe/Sortino metrics
- `cashflow-optimizer` - Income allocation rules for emergency fund, debt payoff, investing
- `asset-allocation-backtester` - Historical backtests for allocation strategies
- `client-wealth-dashboard` - KPI dashboard for net worth, liabilities, and scenario planning
- `financial-data-pipeline` - ETL for market/economic data with scheduled updates

## Next hardening
- Add Entra ID auth and role-based access
- Replace mock adapters with real CRM/portfolio integrations
- Implement production Office file generation + document storage
- Add environment-specific module registry (dev/test/prod)
