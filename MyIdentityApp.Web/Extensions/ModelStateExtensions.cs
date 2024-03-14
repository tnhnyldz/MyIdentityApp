﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MyIdentityApp.Web.Extensions
{
	public static class ModelStateExtensions
	{
		public static void AddModelErrorList(this ModelStateDictionary modelState, List<string> errors)
		{
			errors.ForEach(x =>
			{
				modelState.AddModelError(string.Empty, x);
			});
		}
	}
}
