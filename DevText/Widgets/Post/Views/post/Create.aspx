<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Post.Model.post>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<h2>Create</h2>

	<% using (Html.BeginForm()) {%>
		<%: Html.ValidationSummary(true) %>

		<fieldset>
			<legend>Fields</legend>
			
			<div class="editor-label">
				<%: Html.LabelFor(model => model.Title) %>
			</div>
			<div class="editor-field">
				<%: Html.TextBoxFor(model => model.Title) %>
				<%: Html.ValidationMessageFor(model => model.Title) %>
			</div>
			
			<div class="editor-label">
				<%: Html.LabelFor(model => model.Content) %>
			</div>
			<div class="editor-field">
				<%: Html.TextBoxFor(model => model.Content) %>
				<%: Html.ValidationMessageFor(model => model.Content) %>
			</div>
			
			<div class="editor-label">
				<%: Html.LabelFor(model => model.Author) %>
			</div>
			<div class="editor-field">
				<%: Html.TextBoxFor(model => model.Author) %>
				<%: Html.ValidationMessageFor(model => model.Author) %>
			</div>
			
			<div class="editor-label">
				<%: Html.LabelFor(model => model.Publish) %>
			</div>
			<div class="editor-field">
				<%: Html.TextBoxFor(model => model.Publish) %>
				<%: Html.ValidationMessageFor(model => model.Publish) %>
			</div>
			
			<div class="editor-label">
				<%: Html.LabelFor(model => model.Id) %>
			</div>
			<div class="editor-field">
				<%: Html.TextBoxFor(model => model.Id) %>
				<%: Html.ValidationMessageFor(model => model.Id) %>
			</div>
			
			<p>
				<input type="submit" value="Create" />
			</p>
		</fieldset>

	<% } %>

	<div>
		<%: Html.ActionLink("Back to List", "Index") %>
	</div>

</asp:Content>

