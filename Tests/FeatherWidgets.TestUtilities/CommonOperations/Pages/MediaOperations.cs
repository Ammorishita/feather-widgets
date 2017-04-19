﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Telerik.Sitefinity;
using Telerik.Sitefinity.Libraries.Model;
using Telerik.Sitefinity.Modules.Libraries;

namespace FeatherWidgets.TestUtilities.CommonOperations.Pages
{
    public class MediaOperations
    {
        /// <summary>
        /// Uploads the document in folder.
        /// </summary>
        /// <param name="folderId">The folder id.</param>
        /// <param name="documentTitle">The document title.</param>
        /// <param name="documentResource">The document resource.</param>
        /// <param name="documentExtension">The document extension.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public Guid UploadDocumentInFolder(Guid folderId, string documentTitle, string documentResource, string documentExtension = null)
        {
            var manager = LibrariesManager.GetManager();

            var folder = manager.GetFolder(folderId);
            Library library = this.GetLibraryByFolder(manager, folder);

            var document = manager.CreateDocument();
            var title = documentTitle;
            document.Parent = library;
            if (folderId != library.Id)
                document.FolderId = folderId;
            document.Title = title;
            document.UrlName = title.ToLower().Replace(' ', '-');
            document.ApprovalWorkflowState = "Published";
            manager.RecompileItemUrls<Telerik.Sitefinity.Libraries.Model.Document>(document);

            System.Reflection.Assembly thisExe;
            thisExe = System.Reflection.Assembly.GetExecutingAssembly();
            System.IO.Stream documentStream = thisExe.GetManifestResourceStream(documentResource);

            manager.Upload(document, documentStream, documentExtension ?? Path.GetExtension(documentResource));

            manager.Lifecycle.Publish(document);

            manager.SaveChanges();

            return document.Id;
        }

        /// <summary>
        /// Uploads the video in folder.
        /// </summary>
        /// <param name="folderId">The folder id.</param>
        /// <param name="videoTitle">The video title.</param>
        /// <param name="videoResource">The video resource.</param>
        /// <param name="videoExtension">The video extension.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public Guid UploadVideoInFolder(Guid folderId, string videoTitle, string videoResource, string videoExtension = null)
        {
            var manager = LibrariesManager.GetManager();

            var folder = manager.GetFolder(folderId);
            Library library = this.GetLibraryByFolder(manager, folder);

            var video = manager.CreateVideo();
            var title = videoTitle;
            video.Parent = library;
            if (folderId != library.Id)
                video.FolderId = folderId;
            video.Title = title;
            video.UrlName = title.ToLower().Replace(' ', '-');
            video.ApprovalWorkflowState = "Published";
            manager.RecompileItemUrls<Telerik.Sitefinity.Libraries.Model.Video>(video);

            System.Reflection.Assembly thisExe;
            thisExe = System.Reflection.Assembly.GetExecutingAssembly();
            System.IO.Stream videoStream = thisExe.GetManifestResourceStream(videoResource);

            manager.Upload(video, videoStream, videoExtension ?? Path.GetExtension(videoResource));

            manager.Lifecycle.Publish(video);

            manager.SaveChanges();

            return video.Id;
        }

        /// <summary>
        /// Uploads the image in folder.
        /// </summary>
        /// <param name="folderId">The folder id.</param>
        /// <param name="videoTitle">The image title.</param>
        /// <param name="videoResource">The image resource.</param>
        /// <param name="videoExtension">The image extension.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public Guid UploadImageInFolder(Guid folderId, string imageTitle, string imageResource, string imageExtension = null)
        {
            var manager = LibrariesManager.GetManager();

            var folder = manager.GetFolder(folderId);
            Library library = this.GetLibraryByFolder(manager, folder);

            var image = manager.CreateImage();
            var title = imageTitle;
            image.Parent = library;
            if (folderId != library.Id)
                image.FolderId = folderId;
            image.Title = title;
            image.UrlName = title.ToLower().Replace(' ', '-');
            image.AlternativeText = title;
            image.ApprovalWorkflowState = "Published";
            manager.RecompileItemUrls<Telerik.Sitefinity.Libraries.Model.Image>(image);

            System.Reflection.Assembly thisExe;
            thisExe = System.Reflection.Assembly.GetExecutingAssembly();
            System.IO.Stream imageStream = thisExe.GetManifestResourceStream(imageResource);

            manager.Upload(image, imageStream, imageExtension ?? Path.GetExtension(imageResource));

            manager.Lifecycle.Publish(image);

            manager.SaveChanges();

            return image.Id;
        }

        /// <summary>
        /// Get the Library of a given Folder
        /// </summary>
        /// <param name="manager">Library Manager</param>
        /// <param name="folder">The folder</param>
        /// <returns>The Library containing the folder</returns>
        private Library GetLibraryByFolder(LibrariesManager manager, IFolder folder)
        {
            while (!(folder is Library))
            {
                folder = manager.GetFolder(folder.ParentId);
            }

            var folderToBeCast = folder;

            return folderToBeCast as Library;
        }
    }
}
