using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApiManagerFor<GenericApiModel> : ApiManager  where GenericApiModel : ApiModel
{
    [Header("The API Model attached to this API Manager")]
    public GenericApiModel apiModel;

    public ApiManagerFor()
    {
        base.SetModel(apiModel);
    }
}
