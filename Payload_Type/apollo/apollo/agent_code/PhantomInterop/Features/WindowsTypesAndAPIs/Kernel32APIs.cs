using PhantomInterop.Enums;

namespace PhantomInterop.Features.WindowsTypesAndAPIs;

public class EfficientCoordinatorB33D
{
    public delegate APIInteropTypes.HANDLE OpenProcess(Win32.ProcessAccessFlags dwDesiredAccess, bool bInheritHandle,  int dwProcessId);
    public delegate bool CloseHandle(APIInteropTypes.HANDLE hObject);
}