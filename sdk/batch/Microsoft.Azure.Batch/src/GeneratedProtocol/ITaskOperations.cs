// <auto-generated>
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Microsoft.Azure.Batch.Protocol
{
    using Microsoft.Rest;
    using Microsoft.Rest.Azure;
    using Models;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// TaskOperations operations.
    /// </summary>
    public partial interface ITaskOperations
    {
        /// <summary>
        /// Adds a Task to the specified Job.
        /// </summary>
        /// <remarks>
        /// The maximum lifetime of a Task from addition to completion is 180
        /// days. If a Task has not completed within 180 days of being added it
        /// will be terminated by the Batch service and left in whatever state
        /// it was in at that time.
        /// </remarks>
        /// <param name='jobId'>
        /// The ID of the Job to which the Task is to be added.
        /// </param>
        /// <param name='task'>
        /// The Task to be added.
        /// </param>
        /// <param name='taskAddOptions'>
        /// Additional parameters for the operation
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="BatchErrorException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<AzureOperationHeaderResponse<TaskAddHeaders>> AddWithHttpMessagesAsync(string jobId, TaskAddParameter task, TaskAddOptions taskAddOptions = default(TaskAddOptions), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Lists all of the Tasks that are associated with the specified Job.
        /// </summary>
        /// <remarks>
        /// For multi-instance Tasks, information such as affinityId,
        /// executionInfo and nodeInfo refer to the primary Task. Use the list
        /// subtasks API to retrieve information about subtasks.
        /// </remarks>
        /// <param name='jobId'>
        /// The ID of the Job.
        /// </param>
        /// <param name='taskListOptions'>
        /// Additional parameters for the operation
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="BatchErrorException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.SerializationException">
        /// Thrown when unable to deserialize the response
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<AzureOperationResponse<IPage<CloudTask>,TaskListHeaders>> ListWithHttpMessagesAsync(string jobId, TaskListOptions taskListOptions = default(TaskListOptions), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Adds a collection of Tasks to the specified Job.
        /// </summary>
        /// <remarks>
        /// Note that each Task must have a unique ID. The Batch service may
        /// not return the results for each Task in the same order the Tasks
        /// were submitted in this request. If the server times out or the
        /// connection is closed during the request, the request may have been
        /// partially or fully processed, or not at all. In such cases, the
        /// user should re-issue the request. Note that it is up to the user to
        /// correctly handle failures when re-issuing a request. For example,
        /// you should use the same Task IDs during a retry so that if the
        /// prior operation succeeded, the retry will not create extra Tasks
        /// unexpectedly. If the response contains any Tasks which failed to
        /// add, a client can retry the request. In a retry, it is most
        /// efficient to resubmit only Tasks that failed to add, and to omit
        /// Tasks that were successfully added on the first attempt. The
        /// maximum lifetime of a Task from addition to completion is 180 days.
        /// If a Task has not completed within 180 days of being added it will
        /// be terminated by the Batch service and left in whatever state it
        /// was in at that time.
        /// </remarks>
        /// <param name='jobId'>
        /// The ID of the Job to which the Task collection is to be added.
        /// </param>
        /// <param name='value'>
        /// The collection of Tasks to add. The maximum count of Tasks is 100.
        /// The total serialized size of this collection must be less than 1MB.
        /// If it is greater than 1MB (for example if each Task has 100's of
        /// resource files or environment variables), the request will fail
        /// with code 'RequestBodyTooLarge' and should be retried again with
        /// fewer Tasks.
        /// </param>
        /// <param name='taskAddCollectionOptions'>
        /// Additional parameters for the operation
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="BatchErrorException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.SerializationException">
        /// Thrown when unable to deserialize the response
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<AzureOperationResponse<TaskAddCollectionResult,TaskAddCollectionHeaders>> AddCollectionWithHttpMessagesAsync(string jobId, IList<TaskAddParameter> value, TaskAddCollectionOptions taskAddCollectionOptions = default(TaskAddCollectionOptions), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Deletes a Task from the specified Job.
        /// </summary>
        /// <remarks>
        /// When a Task is deleted, all of the files in its directory on the
        /// Compute Node where it ran are also deleted (regardless of the
        /// retention time). For multi-instance Tasks, the delete Task
        /// operation applies synchronously to the primary task; subtasks and
        /// their files are then deleted asynchronously in the background.
        /// </remarks>
        /// <param name='jobId'>
        /// The ID of the Job from which to delete the Task.
        /// </param>
        /// <param name='taskId'>
        /// The ID of the Task to delete.
        /// </param>
        /// <param name='taskDeleteOptions'>
        /// Additional parameters for the operation
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="BatchErrorException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<AzureOperationHeaderResponse<TaskDeleteHeaders>> DeleteWithHttpMessagesAsync(string jobId, string taskId, TaskDeleteOptions taskDeleteOptions = default(TaskDeleteOptions), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Gets information about the specified Task.
        /// </summary>
        /// <remarks>
        /// For multi-instance Tasks, information such as affinityId,
        /// executionInfo and nodeInfo refer to the primary Task. Use the list
        /// subtasks API to retrieve information about subtasks.
        /// </remarks>
        /// <param name='jobId'>
        /// The ID of the Job that contains the Task.
        /// </param>
        /// <param name='taskId'>
        /// The ID of the Task to get information about.
        /// </param>
        /// <param name='taskGetOptions'>
        /// Additional parameters for the operation
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="BatchErrorException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.SerializationException">
        /// Thrown when unable to deserialize the response
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<AzureOperationResponse<CloudTask,TaskGetHeaders>> GetWithHttpMessagesAsync(string jobId, string taskId, TaskGetOptions taskGetOptions = default(TaskGetOptions), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Updates the properties of the specified Task.
        /// </summary>
        /// <param name='jobId'>
        /// The ID of the Job containing the Task.
        /// </param>
        /// <param name='taskId'>
        /// The ID of the Task to update.
        /// </param>
        /// <param name='constraints'>
        /// Constraints that apply to this Task. If omitted, the Task is given
        /// the default constraints. For multi-instance Tasks, updating the
        /// retention time applies only to the primary Task and not subtasks.
        /// </param>
        /// <param name='taskUpdateOptions'>
        /// Additional parameters for the operation
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="BatchErrorException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<AzureOperationHeaderResponse<TaskUpdateHeaders>> UpdateWithHttpMessagesAsync(string jobId, string taskId, TaskConstraints constraints = default(TaskConstraints), TaskUpdateOptions taskUpdateOptions = default(TaskUpdateOptions), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Lists all of the subtasks that are associated with the specified
        /// multi-instance Task.
        /// </summary>
        /// <remarks>
        /// If the Task is not a multi-instance Task then this returns an empty
        /// collection.
        /// </remarks>
        /// <param name='jobId'>
        /// The ID of the Job.
        /// </param>
        /// <param name='taskId'>
        /// The ID of the Task.
        /// </param>
        /// <param name='taskListSubtasksOptions'>
        /// Additional parameters for the operation
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="BatchErrorException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.SerializationException">
        /// Thrown when unable to deserialize the response
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<AzureOperationResponse<CloudTaskListSubtasksResult,TaskListSubtasksHeaders>> ListSubtasksWithHttpMessagesAsync(string jobId, string taskId, TaskListSubtasksOptions taskListSubtasksOptions = default(TaskListSubtasksOptions), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Terminates the specified Task.
        /// </summary>
        /// <remarks>
        /// When the Task has been terminated, it moves to the completed state.
        /// For multi-instance Tasks, the terminate Task operation applies
        /// synchronously to the primary task; subtasks are then terminated
        /// asynchronously in the background.
        /// </remarks>
        /// <param name='jobId'>
        /// The ID of the Job containing the Task.
        /// </param>
        /// <param name='taskId'>
        /// The ID of the Task to terminate.
        /// </param>
        /// <param name='taskTerminateOptions'>
        /// Additional parameters for the operation
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="BatchErrorException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<AzureOperationHeaderResponse<TaskTerminateHeaders>> TerminateWithHttpMessagesAsync(string jobId, string taskId, TaskTerminateOptions taskTerminateOptions = default(TaskTerminateOptions), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Reactivates a Task, allowing it to run again even if its retry
        /// count has been exhausted.
        /// </summary>
        /// <remarks>
        /// Reactivation makes a Task eligible to be retried again up to its
        /// maximum retry count. The Task's state is changed to active. As the
        /// Task is no longer in the completed state, any previous exit code or
        /// failure information is no longer available after reactivation. Each
        /// time a Task is reactivated, its retry count is reset to 0.
        /// Reactivation will fail for Tasks that are not completed or that
        /// previously completed successfully (with an exit code of 0).
        /// Additionally, it will fail if the Job has completed (or is
        /// terminating or deleting).
        /// </remarks>
        /// <param name='jobId'>
        /// The ID of the Job containing the Task.
        /// </param>
        /// <param name='taskId'>
        /// The ID of the Task to reactivate.
        /// </param>
        /// <param name='taskReactivateOptions'>
        /// Additional parameters for the operation
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="BatchErrorException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<AzureOperationHeaderResponse<TaskReactivateHeaders>> ReactivateWithHttpMessagesAsync(string jobId, string taskId, TaskReactivateOptions taskReactivateOptions = default(TaskReactivateOptions), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Lists all of the Tasks that are associated with the specified Job.
        /// </summary>
        /// <remarks>
        /// For multi-instance Tasks, information such as affinityId,
        /// executionInfo and nodeInfo refer to the primary Task. Use the list
        /// subtasks API to retrieve information about subtasks.
        /// </remarks>
        /// <param name='nextPageLink'>
        /// The NextLink from the previous successful call to List operation.
        /// </param>
        /// <param name='taskListNextOptions'>
        /// Additional parameters for the operation
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="BatchErrorException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.SerializationException">
        /// Thrown when unable to deserialize the response
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<AzureOperationResponse<IPage<CloudTask>,TaskListHeaders>> ListNextWithHttpMessagesAsync(string nextPageLink, TaskListNextOptions taskListNextOptions = default(TaskListNextOptions), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
    }
}