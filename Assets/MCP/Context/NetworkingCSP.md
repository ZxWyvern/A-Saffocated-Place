# Networking Content Security Policy — LAST SHIFT

## Allowed Endpoints & Network Access

- **Runtime Game Client:**
  - `allowed_endpoints: []` (Empty list).
  - The runtime game client runs 100% offline. It must never initiate external HTTP/HTTPS requests, WebSocket connections, or UDP/TCP socket traffic.
  - No remote analytics, telemetry endpoints, or advertising SDKs are permitted in production builds.

## Developer & Editor Tooling Security

- **AB Unity MCP Bridge:**
  - **Host Interface:** `127.0.0.1` (Local loopback only).
  - **Default Port:** `7890`.
  - **Access Restriction:** Bound strictly to local processes. Must reject non-localhost network connections.
- **Local Data Validation:**
  - Playtest telemetry data (`TelemetryService`) is written directly to local application data or project disk storage in standard CSV format (ACC-19).
  - No telemetry data or logs contain sensitive user credentials, personal identifiable information (PII), or machine secrets.
