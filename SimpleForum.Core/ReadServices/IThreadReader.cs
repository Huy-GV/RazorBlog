﻿using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleForum.Core.Communication;
using SimpleForum.Core.Data.Dtos;

namespace SimpleForum.Core.ReadServices;

public interface IThreadReader
{
    /// <summary>
    /// Retrieves a collection of thread entries based on specified criteria asynchronously.
    /// </summary>
    /// <param name="searchString">Optional. The search string to filter thread entries.</param>
    /// <param name="page">Optional. The page number for paginated results (default is 0).</param>
    /// <param name="pageSize">Optional. The number of entries per page (default is 10).</param>
    /// <returns>
    /// The task result contains a read-only collection of <see cref="IndexThreadDto"/> representing the retrieved thread entries.
    /// </returns>
    Task<IReadOnlyCollection<IndexThreadDto>> GetThreadsAsync(
        string? searchString = null,
        int page = 0,
        int pageSize = 10);

    /// <summary>
    /// Retrieves a detailed thread entry based on its identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the thread entry to retrieve.</param>
    /// <returns>
    /// The task result contains a tuple with
    /// a <see cref="ServiceResultCode"/> indicating the result of the operation,
    /// and a <see cref="DetailedThreadDto"/> representing the detailed thread entry (null if not found).
    /// </returns>
    Task<(ServiceResultCode, DetailedThreadDto?)> GetThreadAsync(int id, string requestUserName);

    /// <summary>
    /// Retrieves a detailed thread entry based on its identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the thread entry to retrieve.</param>
    /// <returns>
    /// The task result contains a tuple with
    /// a <see cref="ServiceResultCode"/> indicating the result of the operation,
    /// and a <see cref="DetailedThreadDto"/> representing the detailed thread entry (null if not found).
    /// </returns>
    Task<(ServiceResultCode, IReadOnlyCollection<HiddenThreadDto>)> GetReportTicketAsync(
        string authorUserName,
        string requestUserName);
}
