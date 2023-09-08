PACKAGE_DIR := Packages/com.teach310.core-bluetooth-for-unity

.PHONY: help
#: ヘルプコマンド
help:
	@grep -A1 -E "^#:" Makefile \
	| grep -v -- -- \
	| sed 'N;s/\n/###/' \
	| sed -n 's/^#: \(.*\)###\(.*\):.*/\2###\1/p' \
	| column -t  -s '###'

.PHONY: format
format:
	dotnet format CoreBluetoothForUnity.sln

# .PHONY: mv/samples
mv/samples:
	mv -f ./Assets/CoreBluetooth/Samples/ ./${PACKAGE_DIR}/Samples~/
