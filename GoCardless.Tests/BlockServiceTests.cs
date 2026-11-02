using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GoCardless.Resources;
using GoCardless.Services;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace GoCardless.Tests
{
    public class BlockServiceTests
    {
        private GoCardlessClient client;
        public MockHttp mockHttp;

        [SetUp]
        public void SetUp()
        {
            mockHttp = new MockHttp();
            var httpClient = new HttpClient(mockHttp);
            client = GoCardlessClient.Create("access-token", "https://api.example.com", httpClient);
        }

        [Test]
        public async Task ShouldGetBlock()
        {
            var responseFixture = "fixtures/client/block_service/get_response.json";
            mockHttp.EnqueueResponse(200, responseFixture);

            var resp = await client.Blocks.GetAsync("BLC456");
            mockHttp.AssertRequestMade("GET", "/blocks/BLC456");
            TestHelpers.AssertResponseCanSerializeBackToFixture(resp, responseFixture);

            GoCardless.Resources.Block block = resp.Block;
            ClassicAssert.AreEqual(block.Id, "BLC456");
            ClassicAssert.AreEqual(block.BlockType, BlockBlockType.Email);
            ClassicAssert.AreEqual(block.ReasonType, BlockReasonType.NoIntentToPay);
            ClassicAssert.AreEqual(block.ResourceReference, "example@example.com");
            ClassicAssert.AreEqual(block.Active, true);
            ClassicAssert.AreEqual(
                block.CreatedAt.Value.ToString("o"),
                "2021-03-25T17:26:28.3050000+00:00"
            );
        }

        [Test]
        public async Task ShouldBlockByRef()
        {
            var responseFixture = "fixtures/client/block_service/blockbyref_response.json";
            mockHttp.EnqueueResponse(200, responseFixture);

            var request = new BlockBlockByRefRequest()
            {
                ReasonType = BlockBlockByRefRequest.BlockReasonType.NoIntentToPay.ToString(),
                ReferenceType = BlockBlockByRefRequest.BlockReferenceType.Customer.ToString(),
                ReferenceValue = "CU123",
            };
            var resp = await client.Blocks.BlockByRefAsync(request);
            mockHttp.AssertRequestMade("POST", "/blocks/block_by_ref");
            TestHelpers.AssertResponseCanSerializeBackToFixture(resp, responseFixture);

            ClassicAssert.IsNull(resp.Meta.Cursors.Before);
            ClassicAssert.IsNull(resp.Meta.Cursors.After);

            IReadOnlyList<GoCardless.Resources.Block> blocks = resp.Blocks;
            ClassicAssert.AreEqual(blocks[0].Id, "BLC123");
            ClassicAssert.AreEqual(blocks[0].BlockType, BlockBlockType.Email);
            ClassicAssert.AreEqual(blocks[0].ReasonType, BlockReasonType.NoIntentToPay);
            ClassicAssert.AreEqual(blocks[0].ResourceReference, "example@example.com");
            ClassicAssert.AreEqual(blocks[1].Id, "BLC456");
            ClassicAssert.AreEqual(blocks[1].BlockType, BlockBlockType.BankAccount);
            ClassicAssert.AreEqual(blocks[1].ReasonType, BlockReasonType.NoIntentToPay);
            ClassicAssert.AreEqual(blocks[1].ResourceReference, "BA123");
        }
    }
}
