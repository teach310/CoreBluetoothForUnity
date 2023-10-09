using System.Collections.Generic;

namespace CoreBluetooth
{
    /// <summary>
    /// A collection of data and associated behaviors that accomplish a function or feature of a device.
    /// https://developer.apple.com/documentation/corebluetooth/cbservice
    /// </summary>
    public class CBService
    {
        public string UUID { get; }

        /// <summary>
        /// The peripheral to which this service belongs.
        /// </summary>
        public CBPeripheral Peripheral { get; }

        List<CBCharacteristic> _characteristics = new List<CBCharacteristic>();
        public virtual CBCharacteristic[] Characteristics
        {
            get
            {
                return _characteristics?.ToArray();
            }
            set
            {
                throw new System.NotImplementedException("Not available on 'CBService', only available on CBMutableService.");
            }
        }

        internal CBService(string uuid, CBPeripheral peripheral = null)
        {
            this.UUID = uuid;
            this.Peripheral = peripheral;
        }

        internal CBCharacteristic FindCharacteristic(string uuid) => _characteristics.Find(c => c.UUID == uuid);

        internal void UpdateCharacteristics(CBCharacteristic[] characteristics)
        {
            if (_characteristics != null)
            {
                ClearCharacteristics();
            }

            if (characteristics == null)
            {
                _characteristics = null;
                return;
            }

            foreach (var characteristic in characteristics)
            {
                characteristic.UpdateService(this);
                _characteristics.Add(characteristic);
            }
        }

        void ClearCharacteristics()
        {
            foreach (var characteristic in _characteristics)
            {
                characteristic.UpdateService(null);
            }
            _characteristics.Clear();
        }

        public override string ToString() => $"CBService: UUID = {UUID}";
    }
}
