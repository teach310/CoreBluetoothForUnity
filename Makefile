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

.PHONY: package/build
package/build:
	mv -f ./Assets/CoreBluetooth/Samples/ ./${PACKAGE_DIR}/Samples~/
	mv -f README.md ./${PACKAGE_DIR}/README.md

.PHONY: plugins/build
plugins/build:
	make -C Plugins/CoreBluetoothForUnity dylib
	make -C Plugins/CoreBluetoothForUnity framework
