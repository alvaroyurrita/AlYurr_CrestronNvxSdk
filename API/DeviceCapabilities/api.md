# DeviceCapabilities API

| Property Name | Parent Path | Version | Description | Type | Methods | Validation Rules |
|---|---|---|---|---|---|---|
| DeviceCapabilities | /Device/ | 2.0.0 | The DeviceCapabilites object. | Object | N/A | N/A |
| IsConfigFileUploadSupported | /Device/DeviceCapabilities/ | 2.1.0 | A flag to indicate whether configuration retrieval support exists (true) or not (false) in the XiO Cloud® service. | Boolean | GET | N/A |
| IsLogFileUploadSupported | /Device/DeviceCapabilities/ | 2.1.0 | A flag to indicate whether log file retrieval support exists (true) or not (false) in the XiO Cloud service. | Boolean | GET | N/A |
| PortConfig | /Device/DeviceCapabilities/PortConfig/ | 2.0.0 | Indicates the port configuration of the device. | Object | GET | N/A |
| NumberOfDmInputs | /Device/DeviceCapabilities/PortConfig/ | 2.0.0 | The number of DM® inputs in a device. | Numeric (Signed 16-bit) | GET | N/A |
| NumberOfDmOutputs | /Device/DeviceCapabilities/PortConfig/ | 2.3.10 | The number of DM outputs in a device. | Numeric (Unsigned 16-bit) | GET | N/A |
| NumberOfEthernetAdapters | /Device/DeviceCapabilities/PortConfig/ | 2.0.0 | The number of Ethernet adapters in a device. | Numeric (Unsigned 16-bit) | GET | N/A |
| NumberOfHdmiInputs | /Device/DeviceCapabilities/PortConfig/ | 2.0.0 | The number of HDMI® inputs in a device. | Numeric (Unsigned 16-bit) | GET | N/A |
| NumberOfHdmiOutputs | /Device/DeviceCapabilities/PortConfig/ | 2.0.0 | The number of HDMI outputs in a device. | Numeric (Unsigned 16-bit) | GET | N/A |
| NumberOfUsbCInputs | /Device/DeviceCapabilities/PortConfig/ | 2.3.10 | The number of USB-C® inputs in a device. | Numeric (Unsigned 16-bit) | GET | N/A |
| NumberOfUsbDevicePorts | /Device/DeviceCapabilities/PortConfig/ | 2.3.10 | The number of USB device ports in a device. | Numeric (Unsigned 16-bit) | GET | N/A |
| NumberOfUsbHostPorts | /Device/DeviceCapabilities/PortConfig/ | 2.3.10 | The number of USB host ports in a device. | Numeric (Unsigned 16-bit) | GET | N/A |
| IsAncillaryDeviceFeatureSupported | /Device/DeviceCapabilities/ | 2.2.0 | Flag to indicate if the device supports relaying data from Ancillary Devices (via the CCD framework) to the cloud. | Boolean | GET | N/A |
| AncillaryDevicesSupportedCount | /Device/DeviceCapabilities/ | 2.3.0 | Indicates the number of Ancillary Devices supported by this device. | Numeric (Unsigned 16-bit) | GET | N/A |
| IsDeviceMonitoringSupported | /Device/DeviceCapabilities/ | 2.2.0 | Flag to indicate if the device supports Monitoring devices via ICMP, TCP, Ping, and SNMP for use with the cloud. | Boolean | GET | N/A |
| MonitoringDevicesSupportedCount | /Device/DeviceCapabilities/ | 2.3.0 | Indicates the number of Monitored Devices supported by this device. | Numeric (Unsigned 16-bit) | GET | N/A |
| CCDFrameworkVersionSupported | /Device/DeviceCapabilities/ | 2.3.1 | Indicates the maximum CCD Framework version supported on this box. This is used on the VC-4 for the Enterprise Gateway feature and is used to query the RAD portal for the drivers which support this version of framework and less. This is used for a compatibility check to ensure that the CCD driver which we pull from the portal will actually run on the device. | String | GET | N/A |
| IsFeatureEnablementSupported | /Device/DeviceCapabilities/ | 2.3.2 | Indicates whether the device supports the new XiO Cloud licensing mechanism. | Boolean | GET | N/A |
| Version | /Device/DeviceCapabilities/ | 2.3.10 | The object version. "2.0.0": Initial release. "2.1.0": LogFile and Config File Supported Values for XiO Cloud. "2.2.0": Added IsAncillaryDeviceFeatureSupported, IsDeviceMonitoringSupported. "2.3.0": Added AncillaryDevicesSupportedCount, MonitoringDevicesSupportedCount. "2.3.1": Added CCDFrameworkVersionSupported, "2.3.10": Added NumberOfUsbCInputs, NumberOfUsbDevicePorts, NumberOfUsbHostPorts properties, NumberOfDmOutputs | String | GET | N/A |
