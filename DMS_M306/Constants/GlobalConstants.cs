using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DMS_M306.Constants
{
    public class GlobalConstants
    {
        public const string AlertErrorMessageKey = "AlertErrorMessage";
        public const string AlertWarnMessageKey = "AlertWarnMessage";
        public const string AlertInfoMessageKey = "AlertInfoMessage";
        public const string AlertSuccessMessageKey = "AlertSuccessMessage";

        public const string CreateCategoryFirstMessage = "Please create a category first.";
        public const string CategoryCreateSuccess = "Category successfully created.";
        public const string CategoryCreateError = "An error occurred while creating a category.";
        public const string CategoryDeleteError = "An error occurred while removing a category.";
        public const string CategoryDeleteSuccess = "Category successfully removed.";
        public const string CategoryDeleteDisabledMessage = "Deleting a category is not supported at the moment.";
        public const string FileCreateError = "An error occurred while creating a File.";
        public const string FileCreateSuccess = "File successfully created.";
        public const string FileEditError = "An error occurred while updating a File.";
        public const string FileEditSuccess = "File successfully updated.";
        public const string FileEditNoChanges = "No changes where made.";
        public const string ReleaseCreateSuccess = "Release successfully created.";
        public const string ReleaseCreateError = "An error occurred while creating a Release.";
    }
}