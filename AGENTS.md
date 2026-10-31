# Repository Instructions — A Saffocated Place (LAST SHIFT)

<!-- ai-workflow:begin -->
## AI Workflow Instructions
- Startup: Read docs/ai-workflow/agent.md for operating loop, architecture policy, and task routing.
- Authority: `last-shift-gdd/` is the single source of truth (SSOT v1.1). Code, parameters, and rules must conform to GDD files. Tuning numbers come strictly from `last-shift-gdd/14-parameters.md`.
- Architecture: ARC-01..ARC-05 (pure C# logic separated from Unity API; no global singletons or FindObjectOfType; ScriptableObjects for configuration).
- Session handoff: Read and update `docs/ai-workflow/sessions/main__antigravity.md` (or your branch/actor slug). Never write a shared handoff.
- Navigation: Use `python docs/ai-workflow/tools/context.py .` to select task routes and view source skeletons.
- Verification: Validate changes with `python docs/ai-workflow/tools/validate_pack.py .` and `python docs/ai-workflow/tools/verify_compile.py .`.
- Protected areas: Respect P-001, P-002, P-003, P-004, P-005 documented in `docs/ai-workflow/project-context.md`.
<!-- ai-workflow:end -->
