using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApiManagerFor<ApiModel> : ApiManager  where ApiModel : AbstractQuizzStructure
{

    public ApiModel apiModel;

    public ApiManagerFor()
    {
    }
}
