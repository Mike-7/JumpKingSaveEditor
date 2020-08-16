MONOWASM = C:\Users\Mike\Desktop\mono-wasm

JumpKing.dll: src\JumpKing.cs src\Save.cs src\index.html src\script.js
	mcs /target:library -out:JumpKing.dll /noconfig /nostdlib /r:$(MONOWASM)\wasm-bcl\wasm\mscorlib.dll /r:$(MONOWASM)\wasm-bcl\wasm\System.dll /r:$(MONOWASM)\wasm-bcl\wasm\System.Core.dll /r:$(MONOWASM)\wasm-bcl\wasm\Facades\netstandard.dll /r:$(MONOWASM)\wasm-bcl\wasm\System.Net.Http.dll /r:$(MONOWASM)\framework\WebAssembly.Bindings.dll /r:$(MONOWASM)\framework\System.Net.Http.WebAssemblyHttpHandler src\JumpKing.cs src\Save.cs
	mono "$(MONOWASM)\packager.exe" --copy=always --out=publish --asset=src\index.html --asset=src\script.js JumpKing.dll

clean:
	del JumpKing.dll
	rmdir /s /q publish\managed
	del publish\dotnet.js
	del publish\dotnet.wasm
	del publish\index.html
	del publish\mono-config.js
	del publish\runtime.js
	del publish\script.js