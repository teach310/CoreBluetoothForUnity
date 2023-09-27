//
//  BLEPeripheralView.swift
//  CB4UNativeApp
//
//  Created by Taichi Sato on 2023/09/14.
//

import SwiftUI

struct BLEPeripheralView : View {
    @State var blePeripheral: BLEPeripheral?
    
    var body: some View {
        VStack {
            Text("BLEPeripheral")
                .font(.largeTitle)
                .fontWeight(.bold)
                .foregroundColor(.accentColor)
        }
        .padding()
        .onAppear {
            if blePeripheral == nil {
                blePeripheral = BLEPeripheral()
            }
        }
    }
}

struct BLEPeripheralView_Previews: PreviewProvider {
    static var previews: some View {
        BLEPeripheralView()
    }
}
