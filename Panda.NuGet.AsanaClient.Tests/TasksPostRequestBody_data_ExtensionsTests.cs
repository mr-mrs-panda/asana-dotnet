using FluentAssertions;
using Panda.NuGet.AsanaClient.Clients.Tasks;

namespace Panda.NuGet.AsanaClient.Tests;

public class TasksPostRequestBody_data_ExtensionsTests
{
    [Fact]
    public void Name_WhenSet_ShouldStoreInAdditionalData()
    {
        // Arrange
        var data = new TasksPostRequestBody_data();
        const string name = "Test Task Name";

        // Act
        data.Name = name;

        // Assert
        data.Name.Should().Be(name);
        data.AdditionalData.Should().ContainKey("name");
        data.AdditionalData["name"].Should().Be(name);
    }

    [Fact]
    public void Name_WhenSetToNull_ShouldRemoveFromAdditionalData()
    {
        // Arrange
        var data = new TasksPostRequestBody_data { Name = "Test Task" };

        // Act
        data.Name = null;

        // Assert
        data.Name.Should().BeNull();
        data.AdditionalData.Should().NotContainKey("name");
    }

    [Fact]
    public void Name_WhenNotSet_ShouldReturnNull()
    {
        // Arrange
        var data = new TasksPostRequestBody_data();

        // Act
        var name = data.Name;

        // Assert
        name.Should().BeNull();
    }

    [Fact]
    public void Notes_WhenSet_ShouldStoreInAdditionalData()
    {
        // Arrange
        var data = new TasksPostRequestBody_data();
        const string notes = "Test notes content";

        // Act
        data.Notes = notes;

        // Assert
        data.Notes.Should().Be(notes);
        data.AdditionalData.Should().ContainKey("notes");
        data.AdditionalData["notes"].Should().Be(notes);
    }

    [Fact]
    public void Notes_WhenSetToNull_ShouldRemoveFromAdditionalData()
    {
        // Arrange
        var data = new TasksPostRequestBody_data { Notes = "Test notes" };

        // Act
        data.Notes = null;

        // Assert
        data.Notes.Should().BeNull();
        data.AdditionalData.Should().NotContainKey("notes");
    }

    [Fact]
    public void HtmlNotes_WhenSet_ShouldStoreInAdditionalData()
    {
        // Arrange
        var data = new TasksPostRequestBody_data();
        const string htmlNotes = "<p>Test HTML notes</p>";

        // Act
        data.HtmlNotes = htmlNotes;

        // Assert
        data.HtmlNotes.Should().Be(htmlNotes);
        data.AdditionalData.Should().ContainKey("html_notes");
        data.AdditionalData["html_notes"].Should().Be(htmlNotes);
    }

    [Fact]
    public void HtmlNotes_WhenSetToNull_ShouldRemoveFromAdditionalData()
    {
        // Arrange
        var data = new TasksPostRequestBody_data { HtmlNotes = "<p>Test</p>" };

        // Act
        data.HtmlNotes = null;

        // Assert
        data.HtmlNotes.Should().BeNull();
        data.AdditionalData.Should().NotContainKey("html_notes");
    }

    [Fact]
    public void DueOn_WhenSet_ShouldStoreInAdditionalData()
    {
        // Arrange
        var data = new TasksPostRequestBody_data();
        const string dueOn = "2026-12-31";

        // Act
        data.DueOn = dueOn;

        // Assert
        data.DueOn.Should().Be(dueOn);
        data.AdditionalData.Should().ContainKey("due_on");
        data.AdditionalData["due_on"].Should().Be(dueOn);
    }

    [Fact]
    public void DueOn_WhenSetToNull_ShouldRemoveFromAdditionalData()
    {
        // Arrange
        var data = new TasksPostRequestBody_data { DueOn = "2026-12-31" };

        // Act
        data.DueOn = null;

        // Assert
        data.DueOn.Should().BeNull();
        data.AdditionalData.Should().NotContainKey("due_on");
    }

    [Fact]
    public void DueAt_WhenSet_ShouldStoreInAdditionalData()
    {
        // Arrange
        var data = new TasksPostRequestBody_data();
        const string dueAt = "2026-12-31T23:59:59Z";

        // Act
        data.DueAt = dueAt;

        // Assert
        data.DueAt.Should().Be(dueAt);
        data.AdditionalData.Should().ContainKey("due_at");
        data.AdditionalData["due_at"].Should().Be(dueAt);
    }

    [Fact]
    public void DueAt_WhenSetToNull_ShouldRemoveFromAdditionalData()
    {
        // Arrange
        var data = new TasksPostRequestBody_data { DueAt = "2026-12-31T23:59:59Z" };

        // Act
        data.DueAt = null;

        // Assert
        data.DueAt.Should().BeNull();
        data.AdditionalData.Should().NotContainKey("due_at");
    }

    [Fact]
    public void StartOn_WhenSet_ShouldStoreInAdditionalData()
    {
        // Arrange
        var data = new TasksPostRequestBody_data();
        const string startOn = "2026-01-01";

        // Act
        data.StartOn = startOn;

        // Assert
        data.StartOn.Should().Be(startOn);
        data.AdditionalData.Should().ContainKey("start_on");
        data.AdditionalData["start_on"].Should().Be(startOn);
    }

    [Fact]
    public void StartOn_WhenSetToNull_ShouldRemoveFromAdditionalData()
    {
        // Arrange
        var data = new TasksPostRequestBody_data { StartOn = "2026-01-01" };

        // Act
        data.StartOn = null;

        // Assert
        data.StartOn.Should().BeNull();
        data.AdditionalData.Should().NotContainKey("start_on");
    }

    [Fact]
    public void StartAt_WhenSet_ShouldStoreInAdditionalData()
    {
        // Arrange
        var data = new TasksPostRequestBody_data();
        const string startAt = "2026-01-01T09:00:00Z";

        // Act
        data.StartAt = startAt;

        // Assert
        data.StartAt.Should().Be(startAt);
        data.AdditionalData.Should().ContainKey("start_at");
        data.AdditionalData["start_at"].Should().Be(startAt);
    }

    [Fact]
    public void StartAt_WhenSetToNull_ShouldRemoveFromAdditionalData()
    {
        // Arrange
        var data = new TasksPostRequestBody_data { StartAt = "2026-01-01T09:00:00Z" };

        // Act
        data.StartAt = null;

        // Assert
        data.StartAt.Should().BeNull();
        data.AdditionalData.Should().NotContainKey("start_at");
    }

    [Fact]
    public void Completed_WhenSetToTrue_ShouldStoreInAdditionalData()
    {
        // Arrange
        var data = new TasksPostRequestBody_data();

        // Act
        data.Completed = true;

        // Assert
        data.Completed.Should().BeTrue();
        data.AdditionalData.Should().ContainKey("completed");
        data.AdditionalData["completed"].Should().Be(true);
    }

    [Fact]
    public void Completed_WhenSetToFalse_ShouldStoreInAdditionalData()
    {
        // Arrange
        var data = new TasksPostRequestBody_data();

        // Act
        data.Completed = false;

        // Assert
        data.Completed.Should().BeFalse();
        data.AdditionalData.Should().ContainKey("completed");
        data.AdditionalData["completed"].Should().Be(false);
    }

    [Fact]
    public void Completed_WhenSetToNull_ShouldRemoveFromAdditionalData()
    {
        // Arrange
        var data = new TasksPostRequestBody_data { Completed = true };

        // Act
        data.Completed = null;

        // Assert
        data.Completed.Should().BeNull();
        data.AdditionalData.Should().NotContainKey("completed");
    }

    [Fact]
    public void Liked_WhenSetToTrue_ShouldStoreInAdditionalData()
    {
        // Arrange
        var data = new TasksPostRequestBody_data();

        // Act
        data.Liked = true;

        // Assert
        data.Liked.Should().BeTrue();
        data.AdditionalData.Should().ContainKey("liked");
        data.AdditionalData["liked"].Should().Be(true);
    }

    [Fact]
    public void Liked_WhenSetToFalse_ShouldStoreInAdditionalData()
    {
        // Arrange
        var data = new TasksPostRequestBody_data();

        // Act
        data.Liked = false;

        // Assert
        data.Liked.Should().BeFalse();
        data.AdditionalData.Should().ContainKey("liked");
        data.AdditionalData["liked"].Should().Be(false);
    }

    [Fact]
    public void Liked_WhenSetToNull_ShouldRemoveFromAdditionalData()
    {
        // Arrange
        var data = new TasksPostRequestBody_data { Liked = true };

        // Act
        data.Liked = null;

        // Assert
        data.Liked.Should().BeNull();
        data.AdditionalData.Should().NotContainKey("liked");
    }

    [Fact]
    public void ExternalId_WhenSet_ShouldStoreInAdditionalData()
    {
        // Arrange
        var data = new TasksPostRequestBody_data();
        const string externalId = "external-system-123";

        // Act
        data.ExternalId = externalId;

        // Assert
        data.ExternalId.Should().Be(externalId);
        data.AdditionalData.Should().ContainKey("external");
        data.AdditionalData["external"].Should().BeOfType<Dictionary<string, object>>();
        var external = (Dictionary<string, object>)data.AdditionalData["external"];
        external.Should().ContainKey("id");
        external["id"].Should().Be(externalId);
    }

    [Fact]
    public void ExternalId_WhenSetMultipleTimes_ShouldUpdateValue()
    {
        // Arrange
        var data = new TasksPostRequestBody_data { ExternalId = "first-id" };

        // Act
        data.ExternalId = "second-id";

        // Assert
        data.ExternalId.Should().Be("second-id");
    }

    [Fact]
    public void ExternalId_WhenSetToNull_ShouldRemoveIdFromExternal()
    {
        // Arrange
        var data = new TasksPostRequestBody_data { ExternalId = "external-id" };

        // Act
        data.ExternalId = null;

        // Assert
        data.ExternalId.Should().BeNull();
        data.AdditionalData.Should().NotContainKey("external");
    }

    [Fact]
    public void ExternalId_WhenNotSet_ShouldReturnNull()
    {
        // Arrange
        var data = new TasksPostRequestBody_data();

        // Act
        var externalId = data.ExternalId;

        // Assert
        externalId.Should().BeNull();
    }

    [Fact]
    public void MultipleProperties_CanBeSetSimultaneously()
    {
        // Arrange
        var data = new TasksPostRequestBody_data();

        // Act
        data.Name = "Test Task";
        data.Notes = "Test notes";
        data.DueOn = "2026-12-31";
        data.Completed = false;
        data.ExternalId = "ext-123";

        // Assert
        data.Name.Should().Be("Test Task");
        data.Notes.Should().Be("Test notes");
        data.DueOn.Should().Be("2026-12-31");
        data.Completed.Should().BeFalse();
        data.ExternalId.Should().Be("ext-123");
        data.AdditionalData.Should().HaveCount(5);
    }

    [Fact]
    public void Properties_ShouldPersistAcrossMultipleAccesses()
    {
        // Arrange
        var data = new TasksPostRequestBody_data
        {
            Name = "Persistent Task",
            Notes = "Persistent notes"
        };

        // Act
        var name1 = data.Name;
        var name2 = data.Name;
        var notes1 = data.Notes;
        var notes2 = data.Notes;

        // Assert
        name1.Should().Be(name2).And.Be("Persistent Task");
        notes1.Should().Be(notes2).And.Be("Persistent notes");
    }
}
