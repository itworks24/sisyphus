using System;
using System.Collections.Generic;
using System.Net;
using JetBrains.Annotations;
using Sisyphus.itws;

namespace Sisyphus
{
    internal class EnterpriseWsWrapper
    {

        public Nomenclature[] GetNomenclatureArray()
        {
            var errorRepresent = string.Empty;
            var nomenclatureArray = _service.GetNomenclature(ref errorRepresent);
            if (nomenclatureArray == null)
                throw new Exception(errorRepresent);
            return nomenclatureArray;
        }

        public Contractor[] GetConractorsArray()
        {
            var errorRepresent = string.Empty;
            var contractorsArray = _service.GetСontractors(ref errorRepresent);
            if (contractorsArray == null)
                throw new Exception(errorRepresent);
            return contractorsArray;
        }

        public bool SetContractor(Contractor contractor)
        {
            var errorRepresent = "";
            var result = _service.SetContractorBoard(contractor, ref errorRepresent);
            if(!result)
                throw new Exception(errorRepresent);
            return true;
        }

        public bool SetContractors(Contractor[] contractorsArray)
        {
            var errorRepresent = "";
            return _service.SetContractorsBoards(contractorsArray, ref errorRepresent);
        }

        public bool AddCardHistory(CardHistory cardHistory, [NotNull] ref string errorRepresent)
        {
            if (errorRepresent == null) throw new ArgumentNullException(nameof(errorRepresent));
            var newCardHistory = new itws.CardHistory
            {
                CreatorUserName = cardHistory.CreatorUserName,
                CreatorId = cardHistory.CreatorId,
                CreatorEMail = cardHistory.CreatorEMail,
                Labels = cardHistory.LabelsNames.ToArray(),
                CardId = cardHistory.CardId,
                BoardId = cardHistory.BoardId,
                CreationDateTime = cardHistory.CreationDateTime,
                Name = cardHistory.Name,
                Text = cardHistory.Text,
                CardUrl = cardHistory.Url
            };

            var cardRecordList = new List<CardRecord>();
            cardHistory.InWorkActionRecordList.ForEach(t => cardRecordList.Add(new CardRecord
            {
                BeginerId = t.BeginerId,
                BeginerUserName = t.BeginerUserName,
                BeginerEMail = t.BeginerEMail,
                FinisherId = t.FinisherId,
                FinisherUserName = t.FinisherUserName,
                FinisherEMail = t.FinisherEMail,
                StartDateTime = t.StartDateTime,
                FinishDateTime = t.FinishDateTime,
                ListId = t.ListId
            }));
            newCardHistory.InWorkActionRecordList = cardRecordList.ToArray();

            var commentsList = new List<Comment>();
            cardHistory.Comments.ForEach(t => commentsList.Add(new Comment
            {
                UserEMail = t.AuthorEMail,
                UserName = t.AuthorUserName,
                CommentDateTime = t.CommentDateTime,
                Text = t.Text
            }));
            newCardHistory.Comments = commentsList.ToArray();

            errorRepresent = string.Empty;     
            return _service.AddCardHistory(newCardHistory, ref errorRepresent); 
        }

        private readonly itwts_TrelloSync _service;

        public void CloseConnection()
        {
            _service.Dispose();
        }

        public EnterpriseWsWrapper(TrelloCheckerSettings settings)
        {
            _service = new itwts_TrelloSync
            {
                SoapVersion = System.Web.Services.Protocols.SoapProtocolVersion.Soap12,
                Credentials = new NetworkCredential(settings.EnterpriseUserName, settings.EnterprisePassword)
            };
            _service.ToString();
            _service.Url = settings.EnterpriseWsPath;
        }
    }
}
