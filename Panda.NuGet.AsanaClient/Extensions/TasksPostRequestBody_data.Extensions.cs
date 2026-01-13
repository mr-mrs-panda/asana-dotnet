// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Panda.NuGet.AsanaClient.Clients.Tasks
{
    /// <summary>
    /// Partial class extension for TasksPostRequestBody_data to add missing properties
    /// from the Asana API that are not included in the auto-generated OpenAPI spec.
    ///
    /// This file extends the auto-generated partial class without modifying the Clients/ directory.
    /// </summary>
    public partial class TasksPostRequestBody_data
    {
        /// <summary>
        /// Name of the task. This is generally a short sentence fragment that fits on a line in the UI for maximum readability.
        /// However, it can be longer.
        /// </summary>
        public string? Name
        {
            get => AdditionalData.TryGetValue("name", out var value) ? value as string : null;
            set
            {
                if (value != null)
                    AdditionalData["name"] = value;
                else
                    AdditionalData.Remove("name");
            }
        }

        /// <summary>
        /// Free-form textual information associated with the task (i.e. its description).
        /// The text can be formatted as plain text or as rich text in HTML format.
        /// </summary>
        public string? Notes
        {
            get => AdditionalData.TryGetValue("notes", out var value) ? value as string : null;
            set
            {
                if (value != null)
                    AdditionalData["notes"] = value;
                else
                    AdditionalData.Remove("notes");
            }
        }

        /// <summary>
        /// The notes of the task with formatting as HTML.
        /// </summary>
        public string? HtmlNotes
        {
            get => AdditionalData.TryGetValue("html_notes", out var value) ? value as string : null;
            set
            {
                if (value != null)
                    AdditionalData["html_notes"] = value;
                else
                    AdditionalData.Remove("html_notes");
            }
        }

        /// <summary>
        /// Date on which this task is due, or null if the task has no due date.
        /// Format: YYYY-MM-DD
        /// </summary>
        public string? DueOn
        {
            get => AdditionalData.TryGetValue("due_on", out var value) ? value as string : null;
            set
            {
                if (value != null)
                    AdditionalData["due_on"] = value;
                else
                    AdditionalData.Remove("due_on");
            }
        }

        /// <summary>
        /// Date and time on which this task is due, or null if the task has no due time.
        /// Format: ISO 8601 (e.g., "2025-01-15T10:00:00Z")
        /// </summary>
        public string? DueAt
        {
            get => AdditionalData.TryGetValue("due_at", out var value) ? value as string : null;
            set
            {
                if (value != null)
                    AdditionalData["due_at"] = value;
                else
                    AdditionalData.Remove("due_at");
            }
        }

        /// <summary>
        /// Date on which this task starts, or null if the task has no start date.
        /// Format: YYYY-MM-DD
        /// </summary>
        public string? StartOn
        {
            get => AdditionalData.TryGetValue("start_on", out var value) ? value as string : null;
            set
            {
                if (value != null)
                    AdditionalData["start_on"] = value;
                else
                    AdditionalData.Remove("start_on");
            }
        }

        /// <summary>
        /// Date and time on which work begins for the task, or null if the task has no start time.
        /// Format: ISO 8601
        /// </summary>
        public string? StartAt
        {
            get => AdditionalData.TryGetValue("start_at", out var value) ? value as string : null;
            set
            {
                if (value != null)
                    AdditionalData["start_at"] = value;
                else
                    AdditionalData.Remove("start_at");
            }
        }

        /// <summary>
        /// True if the task is currently marked complete, false if not.
        /// </summary>
        public bool? Completed
        {
            get => AdditionalData.TryGetValue("completed", out var value) ? value as bool? : null;
            set
            {
                if (value.HasValue)
                    AdditionalData["completed"] = value.Value;
                else
                    AdditionalData.Remove("completed");
            }
        }

        /// <summary>
        /// Indicates whether the task should be liked by the authorized user or not.
        /// </summary>
        public bool? Liked
        {
            get => AdditionalData.TryGetValue("liked", out var value) ? value as bool? : null;
            set
            {
                if (value.HasValue)
                    AdditionalData["liked"] = value.Value;
                else
                    AdditionalData.Remove("liked");
            }
        }

        /// <summary>
        /// The external data reference. This is a string representing an external ID that can be used to
        /// identify this task in another system.
        /// </summary>
        public string? ExternalId
        {
            get => AdditionalData.TryGetValue("external", out var value) && value is Dictionary<string, object> external
                ? external.TryGetValue("id", out var id) ? id as string : null
                : null;
            set
            {
                if (value != null)
                {
                    if (!AdditionalData.ContainsKey("external"))
                        AdditionalData["external"] = new Dictionary<string, object>();

                    if (AdditionalData["external"] is Dictionary<string, object> external)
                        external["id"] = value;
                }
                else
                {
                    if (AdditionalData.TryGetValue("external", out var externalObj) &&
                        externalObj is Dictionary<string, object> external)
                    {
                        external.Remove("id");
                        if (external.Count == 0)
                            AdditionalData.Remove("external");
                    }
                }
            }
        }
    }
}
