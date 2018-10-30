private void LoadCampaignFormPox(IList<CampaignFormPos> cfps, CampaignFormPosGroup cfpg, Usuario user)
{
    var result = new List<CampaignFormPos>();
    IEnumerable<CampaignFormPos> campaignExists = null;

    // LINQ COM LEFT JOIN PEGANDO O VALOR QUE FOR IGUAL 
    
    if (cfpg != null && cfpg.Id > 0)
    {
        campaignExists = 
            (from posibleExist in cfps
                    join exist in cfpg.CampaignFormPoses on
                    new { CampaingFormId = posibleExist.CampaignForm.Id, posId = posibleExist.Pos.Id }
                        equals
                    new { CampaingFormId = exist.CampaignForm.Id, posId = exist.Pos.Id }
                select exist);

        result.AddRange(campaignExists);
    }

    // LINQ COM LEFT JOIN PEGANDO O QUE FOR DIFERENTE E ALTERANDO O OBJETO NO RETORNO

    IEnumerable<CampaignFormPos> campaignNews = 
            (from newsCampaigns in cfps
                    join existingCampigns in cfpg.CampaignFormPoses on 
                    new {   CampaignFormId = newsCampaigns.CampaignForm.Id, PosId = newsCampaigns.Pos.Id } 
                        equals 
                    new {   CampaignFormId = existingCampigns.CampaignForm.Id, PosId = existingCampigns.Pos.Id } into temp
                    from values in temp.DefaultIfEmpty()
                    where values == null
                select newsCampaigns).Select(newRecord => 
                                {
                                    newRecord.CampaignFormPosGroup = cfpg;
                                    newRecord.CreatedAt = DateTime.Now;
                                    newRecord.InsertedBy = user;
                                    return newRecord;
                                }) .ToList();

    result.AddRange(campaignNews);

    return result;
}
