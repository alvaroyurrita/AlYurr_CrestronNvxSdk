# DeviceInfo API

| Property Name | Parent Path | Version | Description | Type | Methods | Validation Rules |
|---|---|---|---|---|---|---|
| DeviceInfo | /Device/ | 2.0.0 | The top‑level DeviceInfo object. | Object | N/A | N/A |
| Model | /Device/DeviceInfo/ | 2.0.0 | The device model. | String | GET | N/A |
| ModelSubType | /Device/DeviceInfo/ | 2.3.0 | The secondary device model name (if applicable). | String | GET | N/A |
| Category | /Device/DeviceInfo/ | 2.0.0 | The device category. | String | GET | N/A |
| Manufacturer | /Device/DeviceInfo/ | 2.0.0 | The device manufacturer. | String | GET | N/A |
| ModelId | /Device/DeviceInfo/ | 2.3.1 | The ID of the device model. | String | GET | N/A |
| DeviceId | /Device/DeviceInfo/ | 2.0.0 | The unique ID of the device, based on the MAC address. | String | GET | N/A |
| DeviceCertId | /Device/DeviceInfo/ | 2.2.0 | The unique ID provided by a certificate on the device (as a GUID). | String | GET | N/A |
| SerialNumber | /Device/DeviceInfo/ | 2.0.0 | The device serial number. | String | GET | N/A |
| Name | /Device/DeviceInfo/ | 2.0.2 | The device name. | String | GET | N/A |
| DeviceVersion | /Device/DeviceInfo/ | 2.0.0 | The firmware version running on the device. | String | GET | N/A |
| PufVersion | /Device/DeviceInfo/ | 2.0.1 | Indicates the current PUF (Package Update File) version of the device. If the device does not support a PUF, or if a component within the PUF is altered, the version will report as 0.0.0.0. | String | GET | N/A |
| BuildDate | /Device/DeviceInfo/ | 2.0.0 | The build date of the device firmware. | String | GET | N/A |
| DeviceKey | /Device/DeviceInfo/ | 2.0.0 | A unique device key that is used for generating licenses. | String | GET | N/A |
| MacAddress | /Device/DeviceInfo/ | 2.0.2 | The device MAC address. | String | GET | N/A |
| RebootReason | /Device/DeviceInfo/ | 2.1.0 | The reason the device rebooted last. Possible values are "poweron", "watchdog", "manual","unknown", or "firmware". | String | GET | N/A |
| Version | /Device/DeviceInfo/ | 2.3.1 | The object version. "2.0.0": Initial public release, "2.0.1": Added PufVersion, "2.0.2": Added MacAddress, Name is now a R/W field, "2.1.0": Added RebootReason, "2.2.0": Added DeviceCertId property, "2.3.0": Added ModelSubType, changed Name back to a read-only field, "2.3.1": Added ModelId property. | String | GET | N/A |
