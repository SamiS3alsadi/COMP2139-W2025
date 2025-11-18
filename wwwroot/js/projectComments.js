document.addEventListener("DOMContentLoaded", function () {

    const projectId = document.getElementById("projectId").value;

    loadComments();

    //  load comments (ajax get)    
    function loadComments() {
        fetch(`/ProjectManagement/ProjectComment/GetComments?projectId=${projectId}`)
            .then(response => response.json())
            .then(data => {
                const commentsDiv = document.getElementById("comments-section");
                commentsDiv.innerHTML = "";

                data.forEach(c => {
                    commentsDiv.innerHTML += `
                        <div style="padding: 12px; border: 1px solid #ddd; border-radius: 6px; margin-bottom: 12px;">

                            <div style="font-weight: 600; font-size: 16px;">
                                ${c.content}
                            </div>

                            <div style="margin-top: 4px;">
                                <span style="font-weight: bold;">Posted on:</span>
                                <span style="font-style: italic; color: gray;">
                                    ${new Date(c.createdAt).toLocaleString()}
                                </span>
                            </div>

                        </div>
                    `;
                });
            });
    }

    //  add comment (ajax post)
    document.getElementById("addCommentBtn").addEventListener("click", function () {
        const content = document.getElementById("commentContent").value;

        if (content.trim() === "") {
            alert("Comment cannot be empty.");
            return;
        }

        fetch(`/ProjectManagement/ProjectComment/AddComment?projectId=${projectId}&content=${encodeURIComponent(content)}`, {
            method: "POST"
        })
            .then(response => response.json())
            .then(() => {
                document.getElementById("commentContent").value = "";
                loadComments();
            });
    });
});