# advisory-workflow-dashboard

Central advisor/wealth strategist workspace that serves as both:
- the main dashboard application, and
- the index repo for the broader advisory tool suite (loose integration model).

## Platform direction
- Internal desktop browser first
- Module-tile navigation
- Mock data for v1
- Strong module registry for internal modules and external GitHub repos

## Module suites

### Core analysis suite (wealth and portfolio analysis)
- **Wealth Planning Calculator** (external repo, MVP)
  - Retirement, FIRE, tax-aware growth, inflation scenarios
  - GitHub: https://github.com/john-stromberg/wealth-planning-calculator
- **Portfolio Risk Lab** (independent Streamlit app, MVP)
  - Monte Carlo simulation for portfolio risk analysis, VaR, CVaR, and drawdown metrics
  - App: http://localhost:8501
  - GitHub: https://github.com/john-stromberg/portfolio-risk-lab
- **Cashflow Optimizer** (external repo, MVP)
  - Monthly cashflow optimization for emergency reserves, debt payoff, and investing allocation
  - GitHub: https://github.com/john-stromberg/cashflow-optimizer
- **Asset Allocation Backtester** (external repo, MVP)
  - Allocation strategy backtesting with CAGR, volatility, Sharpe ratio, and drawdown metrics
  - GitHub: https://github.com/john-stromberg/asset-allocation-backtester
- **Client Wealth Dashboard** (external repo, MVP)
  - Advisor KPI dashboard for net worth, liquidity, debt ratio, and scenario comparisons
  - GitHub: https://github.com/john-stromberg/client-wealth-dashboard
- **Financial Data Pipeline** (external repo, MVP)
  - Lightweight ETL pipeline for market, economic, and client cashflow feature sets
  - GitHub: https://github.com/john-stromberg/financial-data-pipeline

### Workflow automation suite (advisor workflow + deliverables)
- **Client Meeting Prep Automation** (external repo, MVP)
  - Pre-meeting brief generation, change summaries, action checklists, and Office deliverable workflow outputs
  - GitHub: https://github.com/john-stromberg/client-meeting-prep-automation
- **Proposal Scenario Builder** (external repo, MVP)
  - Compare current vs. proposed client strategy scenarios with Office-ready proposal packet workflow outputs
  - GitHub: https://github.com/john-stromberg/advisor-proposal-scenario-builder
- **Tax-Aware Withdrawal Planner** (external repo, MVP)
  - Bracket-aware withdrawal sequencing with RMD guardrails across taxable, tax-deferred, and Roth accounts
  - GitHub: https://github.com/john-stromberg/tax-aware-withdrawal-planner
- **Household Cashflow Forecaster** (external repo, MVP)
  - Quarterly and yearly liquidity forecasting with market, inflation, and unexpected-expense stress toggles
  - GitHub: https://github.com/john-stromberg/household-cashflow-forecaster
- **Portfolio Policy Compliance Checker** (external repo, MVP)
  - IPS drift, concentration, restricted-holding, and rebalance-trigger rule checks with exception reporting
  - GitHub: https://github.com/john-stromberg/portfolio-policy-compliance-checker
- **Advisor Communication Generator** (external repo, MVP)
  - Plain-English client update drafting with configurable tone and compliance-safe messaging structure
  - GitHub: https://github.com/john-stromberg/advisor-communication-generator
- **Planning Assumptions Governance** (external repo, MVP)
  - Version-controlled assumptions with submit/approve workflow and audit trail visibility
  - GitHub: https://github.com/john-stromberg/planning-assumptions-governance
- **Client Strategy Workbench** (external repo, MVP)
  - Central household goals/constraints/IPS workspace and recommendations
  - GitHub: https://github.com/john-stromberg/client-strategy-workbench

## Registry and integration model
- Module metadata source: `src/api/module-registry/modules.json`
- Registry API: `GET /api/module-registry`
- UI renders module tiles from registry at runtime.
- Internal modules open local routes.
- External tools with a `route` open their hosted UI. Tools without a route open the linked repo.

See `docs/module-registry.md` for onboarding rules.

## Run
1. `dotnet restore client-meeting-prep-office-suite.slnx`
2. `dotnet run --project src/api/ClientMeetingPrep.Api.csproj`
3. Open:
   - Dashboard hub: `/`
   - Meeting Prep module: `/modules/meeting-prep.html`

## Sample workflow today
1. Open dashboard hub at `/`
2. Click **Client Meeting Prep Automation** to generate pre-meeting brief outputs, or click **Proposal Scenario Builder** to build proposal packets.
3. For Meeting Prep Automation: Enter meeting context → Build packet → Review Word/Excel/Outlook/SharePoint deliverable references.
4. For Proposal Builder: Enter client details + planning notes → Create current/proposed scenarios → Run comparison and review deliverable references.

## Planned external repos (next to scaffold)
- (none - both the core analysis suite and workflow automation suite are now MVP scaffolded)

## Next hardening
- Add Entra ID auth and role-based access
- Replace mock adapters with real CRM/portfolio integrations
- Implement production Office file generation + document storage
- Add environment-specific module registry (dev/test/prod)
