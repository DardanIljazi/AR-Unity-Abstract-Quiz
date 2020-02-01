using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApiManagerFor<ApiModel> : ApiManager  where ApiModel : AbstractQuizzStructure
{
    [Header("The API Model attached to this API Manager")]
    public ApiModel apiModel;

    public ApiManagerFor()
    {
    }
}
