# Security and Compliance Notes

- This MVP uses mock adapters and in-memory storage for development only.
- Approval gate is enforced before exports and Outlook draft generation.
- Audit events capture actor, action, timestamp, and metadata.
- Production rollout should include Entra ID auth, role-based authorization, immutable audit storage, and document retention controls.
