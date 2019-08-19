# File Storage
> see https://aka.ms/autorest

## Configuration
``` yaml
# Generate file storage
input-file: ./file-2019-02-02.json
output-folder: ../src/Generated
clear-output-folder: false

# Use the Azure C# Track 2 generator
# use: C:\src\Storage\Swagger\Generator
# We can't use relative paths here, so use a relative path in generate.ps1
azure-track2-csharp: true
```

## Customizations for Track 2 Generator
See the [AutoRest samples](https://github.com/Azure/autorest/tree/master/Samples/3b-custom-transformations)
for more about how we're customizing things.

### x-ms-code-generation-settings
``` yaml
directive:
- from: swagger-document
  where: $.info["x-ms-code-generation-settings"]
  transform: >
    $.namespace = "Azure.Storage.Files";
    $["client-name"] = "FileRestClient";
    $["client-extensions-name"] = "FilesExtensions";
    $["client-model-factory-name"] = "FilesModelFactory";
    $["x-ms-skip-path-components"] = true;
    $["x-ms-include-sync-methods"] = true;
    $["x-ms-public"] = false;
```

### Url
``` yaml
directive:
- from: swagger-document
  where: $.parameters.Url
  transform: >
    $["x-ms-client-name"] = "resourceUri";
    $.format = "url";
    $["x-ms-trace"] = true;
```

### Ignore common headers
``` yaml
directive:
- from: swagger-document
  where: $["x-ms-paths"]..responses..headers["x-ms-request-id"]
  transform: >
    $["x-ms-ignore"] = true;
- from: swagger-document
  where: $["x-ms-paths"]..responses..headers["x-ms-version"]
  transform: >
    $["x-ms-ignore"] = true;
- from: swagger-document
  where: $["x-ms-paths"]..responses..headers["Date"]
  transform: >
    $["x-ms-ignore"] = true;
```

### Clean up Failure response
``` yaml
directive:
- from: swagger-document
  where: $["x-ms-paths"]..responses.default
  transform: >
    $["x-ms-client-name"] = "StorageErrorResult";
    $["x-ms-create-exception"] = true;
    $["x-ms-public"] = false;
    $.headers["x-ms-error-code"]["x-ms-ignore"] = true;
```

### ApiVersionParameter
``` yaml
directive:
- from: swagger-document
  where: $.parameters.ApiVersionParameter
  transform: $.enum = ["2019-02-02"]
```

### /?restype=service&comp=properties
``` yaml
directive:
- from: swagger-document
  where: $.definitions
  transform: >
    if (!$.FileServiceProperties) {
        $.FileServiceProperties = $.StorageServiceProperties;
        delete $.StorageServiceProperties;
        $.FileServiceProperties.xml = { "name": "StorageServiceProperties" };
    }
- from: swagger-document
  where: $.parameters
  transform: >
    if (!$.FileServiceProperties) {
        const props = $.FileServiceProperties = $.StorageServiceProperties;
        props.name = "FileServiceProperties";
        props["x-ms-client-name"] = "properties";
        props.schema = { "$ref": props.schema.$ref.replace(/[#].*$/, "#/definitions/FileServiceProperties") };
        delete $.StorageServiceProperties;
    }
- from: swagger-document
  where: $["x-ms-paths"]["/?restype=service&comp=properties"]
  transform: >
    const param = $.put.parameters[0];
    if (param && param["$ref"] && param["$ref"].endsWith("StorageServiceProperties")) {
        const path = param["$ref"].replace(/[#].*$/, "#/parameters/FileServiceProperties");
        $.put.parameters[0] = { "$ref": path };
    }
    const def = $.get.responses["200"].schema;
    if (def && def["$ref"] && def["$ref"].endsWith("StorageServiceProperties")) {
        const path = def["$ref"].replace(/[#].*$/, "#/definitions/FileServiceProperties");
        $.get.responses["200"].schema = { "$ref": path };
    }
```

### /?comp=list
``` yaml
directive:
- from: swagger-document
  where: $.definitions
  transform: >
    if (!$.SharesSegment) {
        $.SharesSegment = $.ListSharesResponse;
        delete $.ListSharesResponse;
        $.SharesSegment["x-ms-public"] = false;
    }
- from: swagger-document
  where: $["x-ms-paths"]["/?comp=list"]
  transform: >
    const def = $.get.responses["200"].schema;
    if (!def["$ref"].endsWith("SharesSegment")) {
        const path = def["$ref"].replace(/[#].*$/, "#/definitions/SharesSegment");
        $.get.responses["200"].schema = { "$ref": path };
    }
```

### /{shareName}?restype=share
``` yaml
directive:
- from: swagger-document
  where: $["x-ms-paths"]["/{shareName}?restype=share"]
  transform: >
    $.put.responses["201"].description = "Success";
    $.put.responses["201"]["x-ms-client-name"] = "ShareInfo";
    $.get.responses["200"]["x-ms-client-name"] = "ShareProperties";
```

### /{shareName}?restype=share&comp=snapshot
``` yaml
directive:
- from: swagger-document
  where: $["x-ms-paths"]["/{shareName}?restype=share&comp=snapshot"]
  transform: >
    $.put.responses["201"]["x-ms-client-name"] = "ShareSnapshotInfo";
```

### /{shareName}?restype=share&comp=properties
``` yaml
directive:
- from: swagger-document
  where: $["x-ms-paths"]["/{shareName}?restype=share&comp=properties"]
  transform: >
    $.put.responses["200"]["x-ms-client-name"] = "ShareInfo";
    $.put.responses["200"].headers.ETag.description = "The ETag contains a value which represents the version of the share, in quotes.";
    $.put.responses["200"].headers["Last-Modified"].description = "Returns the date and time the share was last modified. Any operation that modifies the share or its properties or metadata updates the last modified time. Operations on files do not affect the last modified time of the share.";
```

### /{shareName}?restype=share&comp=metadata
``` yaml
directive:
- from: swagger-document
  where: $["x-ms-paths"]["/{shareName}?restype=share&comp=metadata"]
  transform: >
    $.put.responses["200"]["x-ms-client-name"] = "ShareInfo";
    $.put.responses["200"].headers.ETag.description = "The ETag contains a value which represents the version of the share, in quotes.";
    $.put.responses["200"].headers["Last-Modified"].description = "Returns the date and time the share was last modified. Any operation that modifies the share or its properties or metadata updates the last modified time. Operations on files do not affect the last modified time of the share.";
```

### /{shareName}?restype=share&comp=acl
``` yaml
directive:
- from: swagger-document
  where: $["x-ms-paths"]["/{shareName}?restype=share&comp=acl"]
  transform: >
    $.get.responses["200"].headers.ETag["x-ms-ignore"] = true;
    $.get.responses["200"].headers["Last-Modified"]["x-ms-ignore"] = true;
    $.put.responses["200"]["x-ms-client-name"] = "ShareInfo";
    $.put.responses["200"].description = "Success";
    $.put.responses["200"].headers.ETag.description = "The ETag contains a value which represents the version of the share, in quotes.";
    $.put.responses["200"].headers["Last-Modified"].description = "Returns the date and time the share was last modified. Any operation that modifies the share or its properties or metadata updates the last modified time. Operations on files do not affect the last modified time of the share.";
- from: swagger-document
  where: $.parameters.ShareAcl
  transform: >
    $["x-ms-client-name"] = "permissions";
```

### /{shareName}?restype=share&comp=stats
``` yaml
directive:
- from: swagger-document
  where: $.definitions
  transform: >
    if (!$.ShareStatistics) {
        $.ShareStatistics = $.ShareStats;
        delete $.ShareStats;
        $.ShareStatistics.xml = { "name": "ShareStats" };
        $.ShareStatistics.properties.ShareUsageBytes.description = "The approximate size of the data stored in bytes, rounded up to the nearest gigabyte. Note that this value may not include all recently created or recently resized files.";
    }
- from: swagger-document
  where: $["x-ms-paths"]["/{shareName}?restype=share&comp=stats"]
  transform: >
    $.get.responses["200"].headers.ETag["x-ms-ignore"] = true;
    $.get.responses["200"].headers["Last-Modified"]["x-ms-ignore"] = true;
    const def = $.get.responses["200"].schema;
    if (!def["$ref"].endsWith("ShareStatistics")) {
        const path = def["$ref"].replace(/[#].*$/, "#/definitions/ShareStatistics");
        $.get.responses["200"].schema = { "$ref": path };
    }
```

### /{shareName}/{directory}?restype=directory
``` yaml
directive:
- from: swagger-document
  where: $["x-ms-paths"]["/{shareName}/{directory}?restype=directory"]
  transform: >
    $.put.responses["201"].headers["x-ms-request-server-encrypted"]["x-ms-ignore"] = true;
    $.put.responses["201"]["x-ms-client-name"] = "StorageDirectoryInfo";
    $.get.responses["200"]["x-ms-client-name"] = "StorageDirectoryProperties";
```

### /{shareName}/{directory}?restype=directory&comp=metadata
``` yaml
directive:
- from: swagger-document
  where: $["x-ms-paths"]["/{shareName}/{directory}?restype=directory&comp=metadata"]
  transform: >
    $.put.responses["200"]["x-ms-client-name"] = "StorageDirectoryInfo";
    $.put.responses["200"].description = "Success, Directory created.";
    $.put.responses["200"].headers["x-ms-request-server-encrypted"]["x-ms-ignore"] = true;
    $.put.responses["200"].headers["Last-Modified"] = {
      "type": "string",
      "format": "date-time-rfc1123",
      "description": "Returns the date and time the share was last modified. Any operation that modifies the directory or its properties updates the last modified time. Operations on files do not affect the last modified time of the directory."
    };
```

### /{shareName}/{directory}?restype=directory&comp=list
``` yaml
directive:
- from: swagger-document
  where: $.definitions
  transform: >
    if (!$.FilesAndDirectoriesSegment) {
        $.FilesAndDirectoriesSegment = $.ListFilesAndDirectoriesSegmentResponse;
        delete $.ListFilesAndDirectoriesSegmentResponse;
        $.FilesAndDirectoriesSegment.required = ["ServiceEndpoint", "ShareName", "DirectoryPath", "NextMarker"];
        const path = $.FilesAndDirectoriesSegment.properties.Segment["$ref"].replace(/[#].*$/, "#/definitions/");
        $.FilesAndDirectoriesSegment.properties.DirectoryItems = {
            "type": "array",
            "items": { "$ref": path + "DirectoryItem" },
            "xml": { "name": "Entries", "wrapped": true }
        };
        $.FilesAndDirectoriesSegment.properties.FileItems = {
            "type": "array",
            "items": { "$ref": path + "FileItem" },
            "xml": { "name": "Entries", "wrapped": true }
        };
        delete $.FilesAndDirectoriesSegment.properties.Segment;
        $.FilesAndDirectoriesSegment["x-ms-public"] = false;
        delete $.FilesAndDirectoriesListSegment;
    }
- from: swagger-document
  where: $["x-ms-paths"]["/{shareName}/{directory}?restype=directory&comp=list"]
  transform: >
    $.get.responses["200"].headers["Content-Type"]["x-ms-ignore"] = true;
    const def = $.get.responses["200"].schema;
    if (!def["$ref"].endsWith("FilesAndDirectoriesSegment")) {
        const path = def["$ref"].replace(/[#].*$/, "#/definitions/FilesAndDirectoriesSegment");
        $.get.responses["200"].schema = { "$ref": path };
    }
```

### StorageHandlesSegment
``` yaml
directive:
- from: swagger-document
  where: $.definitions
  transform: >
    if (!$.StorageHandle) {
        $.StorageHandle = $.HandleItem;
        delete $.HandleItem;
    }
    if (!$.StorageHandlesSegment) {
        $.StorageHandlesSegment = $.ListHandlesResponse;
        delete $.ListHandlesResponse;
        $.StorageHandlesSegment["x-ms-public"] = false;
        const path = $.StorageHandlesSegment.properties.HandleList.items.$ref.replace(/[#].*$/, "#/definitions/");
        $.StorageHandlesSegment.properties.Handles = {
            "type": "array",
            "items": { "$ref": path + "StorageHandle" },
            "xml": { "name": "Entries", "wrapped": true }
        };
        delete $.StorageHandlesSegment.properties.HandleList;
    }
```

### /{shareName}/{directory}?comp=listhandles
``` yaml
directive:
- from: swagger-document
  where: $["x-ms-paths"]["/{shareName}/{directory}?comp=listhandles"]
  transform: >
    $.get.responses["200"].headers["Content-Type"]["x-ms-ignore"] = true;
    const def = $.get.responses["200"].schema;
    if (!def["$ref"].endsWith("StorageHandlesSegment")) {
        const path = def["$ref"].replace(/[#].*$/, "#/definitions/StorageHandlesSegment");
        $.get.responses["200"].schema = { "$ref": path };
    }
```

### /{shareName}/{directory}/{fileName}?comp=listhandles
``` yaml
directive:
- from: swagger-document
  where: $["x-ms-paths"]["/{shareName}/{directory}/{fileName}?comp=listhandles"]
  transform: >
    $.get.responses["200"].headers["Content-Type"]["x-ms-ignore"] = true;
    const def = $.get.responses["200"].schema;
    if (!def["$ref"].endsWith("StorageHandlesSegment")) {
        const path = def["$ref"].replace(/[#].*$/, "#/definitions/StorageHandlesSegment");
        $.get.responses["200"].schema = { "$ref": path };
    }
```

### /{shareName}/{directory}?comp=forceclosehandles
``` yaml
directive:
- from: swagger-document
  where: $["x-ms-paths"]["/{shareName}/{directory}?comp=forceclosehandles"]
  transform: >
    $.put.responses["200"]["x-ms-client-name"] = "StorageClosedHandlesSegment";
```

### /{shareName}/{directory}/{fileName}?comp=forceclosehandles
``` yaml
directive:
- from: swagger-document
  where: $["x-ms-paths"]["/{shareName}/{directory}/{fileName}?comp=forceclosehandles"]
  transform: >
    $.put.responses["200"]["x-ms-client-name"] = "StorageClosedHandlesSegment";
```

### /{shareName}/{directory}/{fileName}
``` yaml
directive:
- from: swagger-document
  where: $["x-ms-paths"]["/{shareName}/{directory}/{fileName}"]
  transform: >
    $.put.responses["201"]["x-ms-client-name"] = "StorageFileInfo";
    $.get.responses["200"].headers["Content-MD5"]["x-ms-client-name"] = "ContentHash";
    $.get.responses["200"].headers["x-ms-copy-source"].format = "url";
    $.get.responses["200"].headers["x-ms-copy-status"]["x-ms-enum"].name = "CopyStatus";
    $.get.responses["200"].headers["x-ms-content-md5"]["x-ms-client-name"] = "FileContentHash";
    $.get.responses["200"]["x-ms-client-name"] = "FlattenedStorageFileProperties";
    $.get.responses["200"]["x-ms-public"] = false;
    $.get.responses["200"]["x-ms-schema-client-name"] = "Content";
    $.get.responses["206"].headers["Content-MD5"]["x-ms-client-name"] = "ContentHash";
    $.get.responses["206"].headers["x-ms-copy-source"].format = "url";
    $.get.responses["206"].headers["x-ms-copy-status"]["x-ms-enum"].name = "CopyStatus";
    $.get.responses["206"].headers["x-ms-content-md5"]["x-ms-client-name"] = "FileContentHash";
    $.get.responses["206"]["x-ms-client-name"] = "FlattenedStorageFileProperties";
    $.get.responses["206"]["x-ms-public"] = false;
    $.get.responses["206"]["x-ms-schema-client-name"] = "Content";
    $.head.responses["200"].headers["Content-MD5"]["x-ms-client-name"] = "ContentHash";
    $.head.responses["200"].headers["Content-Encoding"].type = "array";
    $.head.responses["200"].headers["Content-Encoding"].collectionFormat = "multi";
    $.head.responses["200"].headers["Content-Encoding"].items = { "type": "string" };
    $.head.responses["200"].headers["Content-Language"].type = "array";
    $.head.responses["200"].headers["Content-Language"].collectionFormat = "multi";
    $.head.responses["200"].headers["Content-Language"].items = { "type": "string" };
    $.head.responses["200"].headers["x-ms-copy-status"]["x-ms-enum"].name = "CopyStatus";
    $.head.responses["200"]["x-ms-client-name"] = "StorageFileProperties";
    $.head.responses.default = {
        "description": "Failure",
        "x-ms-client-name": "FailureNoContent",
        "x-ms-create-exception": true,
        "x-ms-public": false,
        "headers": { "x-ms-error-code": { "x-ms-client-name": "ErrorCode", "type": "string" } }
    };
- from: swagger-document
  where: $.parameters.FileContentLanguage
  transform: >
    $.type = "array";
    $.collectionFormat = "multi";
    $.items = { "type": "string" };
- from: swagger-document
  where: $.parameters.FileContentEncoding
  transform: >
    $.type = "array";
    $.collectionFormat = "multi";
    $.items = { "type": "string" };
```

### MD5 to Hash
``` yaml
directive:
- from: swagger-document
  where: $.parameters.ContentMD5["x-ms-client-name"]
  transform: return "contentHash";
- from: swagger-document
  where: $.parameters.FileContentMD5["x-ms-client-name"]
  transform: return "fileContentHash";
- from: swagger-document
  where: $.parameters.GetRangeContentMD5["x-ms-client-name"]
  transform: return "rangeGetContentHash";
```

### /{shareName}/{directory}/{fileName}?comp=properties
``` yaml
directive:
- from: swagger-document
  where: $["x-ms-paths"]["/{shareName}/{directory}/{fileName}?comp=properties"]
  transform: >
    $.put.operationId = "File_SetProperties";
    $.put.responses["200"].description = "Success, File created.";
    $.put.responses["200"].headers["Last-Modified"].description = "Returns the date and time the share was last modified. Any operation that modifies the directory or its properties updates the last modified time. Operations on files do not affect the last modified time of the directory.";
    $.put.responses["200"]["x-ms-client-name"] = "StorageFileInfo";
```

### /{shareName}/{directory}/{fileName}?comp=metadata
``` yaml
directive:
- from: swagger-document
  where: $["x-ms-paths"]["/{shareName}/{directory}/{fileName}?comp=metadata"]
  transform: >
    $.put.responses["200"].description = "Success, File created.";
    $.put.responses["200"].headers["Last-Modified"] = {
        "type": "string",
        "format": "date-time-rfc1123",
        "description": "Returns the date and time the share was last modified. Any operation that modifies the directory or its properties updates the last modified time. Operations on files do not affect the last modified time of the directory."
    };
    $.put.responses["200"]["x-ms-client-name"] = "StorageFileInfo";
```

### /{shareName}/{directory}/{fileName}?comp=range
``` yaml
directive:
- from: swagger-document
  where: $["x-ms-paths"]["/{shareName}/{directory}/{fileName}?comp=range"]
  transform: >
    $.put.responses["201"].headers["Content-MD5"]["x-ms-client-name"] = "ContentHash";
    $.put.responses["201"]["x-ms-client-name"] = "StorageFileUploadInfo";
```

### /{shareName}/{directory}/{fileName}?comp=rangelist
``` yaml
directive:
- from: swagger-document
  where: $["x-ms-paths"]["/{shareName}/{directory}/{fileName}?comp=rangelist"]
  transform: >
    $.get.responses["200"]["x-ms-client-name"] = "StorageFileRangeInfo";
    $.get.responses["200"]["x-ms-schema-client-name"] = "Ranges";
```

### /{shareName}/{directory}/{fileName}?comp=copy
``` yaml
directive:
- from: swagger-document
  where: $["x-ms-paths"]["/{shareName}/{directory}/{fileName}?comp=copy"]
  transform: >
    $.put.responses["202"].headers["x-ms-copy-status"]["x-ms-enum"].name = "CopyStatus";
    $.put.responses["202"]["x-ms-client-name"] = "StorageFileCopyInfo";
- from: swagger-document
  where: $.parameters.CopySource
  transform: >
    $.format = "url";
```

### DirectoryItem
``` yaml
directive:
- from: swagger-document
  where: $.definitions.DirectoryItem
  transform: $["x-ms-public"] = false
```

### FileItem
``` yaml
directive:
- from: swagger-document
  where: $.definitions.FileItem
  transform: $["x-ms-public"] = false
```

### ErrorCode
``` yaml
directive:
- from: swagger-document
  where: $.definitions.ErrorCode["x-ms-enum"]
  transform: >
    $.name = "FileErrorCode";
```

### FileProperty
``` yaml
directive:
- from: swagger-document
  where: $.definitions.FileProperty
  transform: >
    $["x-ms-public"] = false;
```

### Metrics
``` yaml
directive:
- from: swagger-document
  where: $.definitions.Metrics
  transform: >
    $.type = "object";
```

### StorageError
``` yaml
directive:
- from: swagger-document
  where: $.definitions.StorageError
  transform: >
    $["x-ms-public"] = false;
    $.properties.Code = { "type": "string" };
```

### ShareItemProperties
``` yaml
directive:
- from: swagger-document
  where: $.definitions.ShareItem
  transform: >
    const def = $.properties.Properties;
    if (!def["$ref"].endsWith("ShareItemProperties")) {
        const path = def["$ref"].replace(/[#].*$/, "#/definitions/ShareItemProperties");
        $.properties.Properties = { "$ref": path };
    }
```

### ShareItemProperties
``` yaml
directive:
- from: swagger-document
  where: $.definitions
  transform: >
    if (!$.ShareItemProperties) {
        $.ShareItemProperties = $.ShareProperties;
        delete $.ShareProperties;
        $.ShareItemProperties.required = [];
        $.ShareItemProperties.xml = { "name": "Properties" };
    }
```

### FilePermission
``` yaml
directive:
- from: swagger-document
  where: $.parameters.FilePermission
  transform: >
    $.description = "If specified the permission (security descriptor) shall be set for the directory/file. This header can be used if Permission size is &lt;= 8KB, else x-ms-file-permission-key header shall be used. Default value: Inherit. If SDDL is specified as input, it must have owner, group and dacl. Note: Only one of the x-ms-file-permission or x-ms-file-permission-key should be specified.";
```

### Times aren't required
``` yaml
directive:
- from: swagger-document
  where: $.parameters.FileCreationTime
  transform: >
    delete $.format;
- from: swagger-document
  where: $.parameters.FileLastWriteTime
  transform: >
    delete $.format;
```