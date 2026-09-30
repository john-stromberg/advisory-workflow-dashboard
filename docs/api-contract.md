# API Contract

## Meeting Packets
- `POST /api/meeting-packets` create packet draft
- `GET /api/meeting-packets/{id}` get packet
- `POST /api/meeting-packets/{id}/refresh` refresh summary and changes
- `POST /api/meeting-packets/{id}/submit-review` submit for review
- `POST /api/meeting-packets/{id}/approve` approve packet

## Approvals
- `POST /api/meeting-packets/{id}/approvals/submit` submit packet
- `POST /api/meeting-packets/{id}/approvals/approve` approve packet

## Exports
- `POST /api/meeting-packets/{id}/export/excel` export excel artifact
- `POST /api/meeting-packets/{id}/export/powerpoint` export powerpoint artifact
- `POST /api/meeting-packets/{id}/export/outlook-draft` create outlook draft

## Audit
- `GET /api/audit-events?packetId={id}` list audit events for packet

## Demo
- `GET /api/demo/clients` list sample clients
- `POST /api/demo/quickstart` create and auto-approve a sample packet
