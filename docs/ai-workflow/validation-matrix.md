# Validation Matrix — A Saffocated Place (LAST SHIFT)

### V-001 C# project compilation check
Covers: R-001, F-001
Mode: static
Preconditions and environment: .NET SDK and solution files present in workspace
Runner or exact manual input path: python docs/ai-workflow/tools/verify_compile.py .
Expected observable result: Compilation succeeds with 0 errors across project assemblies
Status: passed
Observed result and evidence location: verify_compile.py succeeded with COMPILE_STATUS: OK (0 errors) on Assembly-CSharp.csproj and Assembly-CSharp-Editor.csproj
Remaining limit or next action: Re-run when new C# scripts are added

### V-002 Initial project structure and bootstrap scene verification
Covers: R-001, F-001
Mode: static
Preconditions and environment: Workspace inspection of scenes and package assets
Runner or exact manual input path: python docs/ai-workflow/tools/context.py . --file Assets/Scenes/SampleScene.unity
Expected observable result: Valid development scene with camera and environment volume confirmed
Status: not_run
Observed result and evidence location: none yet
Remaining limit or next action: Transition from SampleScene to dedicated game bootstrap scene

### V-003 CueResolver EditMode unit test suite execution
Covers: R-001, F-002
Mode: EditMode
Preconditions and environment: Unity Editor or command line test runner with NUnit
Runner or exact manual input path: Unity -runTests -testPlatform editmode
Expected observable result: All RES-01 through RES-09 test cases pass
Status: not_run
Observed result and evidence location: none yet
Remaining limit or next action: Awaiting implementation of CueResolver and test assembly

### V-004 Matchstick mixer ducking and timer consistency check
Covers: R-002, F-003
Mode: PlayMode
Preconditions and environment: Unity Editor connected with authored AudioMixer asset
Runner or exact manual input path: Trigger matchstick ignition in test scene; observe mixer snapshot transitions and timer behavior
Expected observable result: Mixer ducks by 12 dB during 1.8s burn, returns to normal upon extinguishing
Status: not_run
Observed result and evidence location: none yet
Remaining limit or next action: AudioMixer asset authoring required

### V-005 Threat state transition and strike accumulation check
Covers: R-003
Mode: EditMode
Preconditions and environment: Unit test runner
Runner or exact manual input path: Simulate cue inputs and verify threat state transitions
Expected observable result: Strikes increment on valid triggers and cap at P-STRIKE-MAX (2)
Status: not_run
Observed result and evidence location: none yet
Remaining limit or next action: Awaiting ThreatStateController implementation
