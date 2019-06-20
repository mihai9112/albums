﻿using System;
using System.Linq;
using System.Reflection;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using RunPath.WebApi.Models;
using BadRequestObjectResult = RunPath.WebApi.Models.BadRequestObjectResult;

namespace RunPath.WebApi.Filters
{
    public class ValidateRequestModelFilter : IActionFilter
    {
        private readonly IValidatorFactory _validatorFactory;
        private const string _requestModelInvalid = "request_invalid";

        public ValidateRequestModelFilter(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory ?? throw new ArgumentNullException(nameof(validatorFactory));
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            foreach (var parameterDescriptor in context.ActionDescriptor.Parameters)
            {
                var parameter = (ControllerParameterDescriptor)parameterDescriptor;
                var typeInfo = parameter.ParameterType.GetTypeInfo();

                if (!typeInfo.IsClass || !context.ActionArguments.TryGetValue(parameter.Name, out var parameterValue))
                    continue;

                switch (parameterValue)
                {
                    case null when !parameter.ParameterInfo.IsOptional:
                    {
                        var validationError = new ValidationError(_requestModelInvalid, new[] {"request_body_required"});
                        context.Result = new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(validationError);
                        return;
                    }
                    case null:
                        return;
                }

                var validator = _validatorFactory.GetValidator(parameter.ParameterType);
                var validationResult = validator?.Validate(parameterValue);

                if (validationResult == null || validationResult.IsValid)
                    continue;

                var errorCodes = validationResult.Errors.Select(e => e.ErrorCode).OrderBy(x => x).ToList();
                var errorResponse = new ValidationError(_requestModelInvalid, errorCodes);
                context.Result = new BadRequestObjectResult(errorResponse);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
