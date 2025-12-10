# AudioVideoInputOutput API

| Property Name | Parent Path | Version | Description | Type | Methods | Validation Rules |
|---|---|---|---|---|---|---|
| AudioVideoInputOutput | /Device/ | 2.0.0 | The top-level AudioVideoInputOutput object | Object | N/A | N/A |
| GlobalConfig | /Device/AudioVideoInputOutput/ | 2.5.0 | The GlobalConfig object. | Object | N/A | N/A |
| GlobalEdid | /Device/AudioVideoInputOutput/GlobalConfig/ | 2.5.0 | Applies EDID to all inputs of a corresponding name. | String | POST | N/A |
| GlobalEdidType | /Device/AudioVideoInputOutput/GlobalConfig/ | 2.5.0 | Applies EDID to all inputs of a corresponding type. | String | POST | "Copy", "System", "Custom" |
| Inputs | /Device/AudioVideoInputOutput/ | 2.4.0 | Contains the inputs supported on the device. Refer to the inputs table below. | Object Collection | N/A | N/A |
| Outputs | /Device/AudioVideoInputOutput/ | 2.4.0 | Contains the outputs supported on the device. Refer to the outputs table below. | Object Collection | N/A | N/A |
| Version | /Device/AudioVideoInputOutput/ | 2.5.0 | The object version. "2.0.0": Initial public release, "2.0.1": Added HdcpTransmitterMode in Output Section, "2.0.2": Added EndpointExists for Inputs and Outputs, "2.0.3": Added ResolutionOptionsName, ResolutionOptionsVersion for Outputs, "2.1.0": Added Orientation for Outputs, "2.2.0": Added VideoTimeout properties and CecControl object, InputControl subobject, "2.3.0": Added EndpointId, IsEndPointOnline, and DM endpoint power control object support, "2.4.0": Added UnsupportedVideoDetected property in both HDMI input and output, "2.5.0": Added the GlobalConfig subobject. | String | GET | N/A |
