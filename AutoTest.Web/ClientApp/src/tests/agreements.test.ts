import { EntrantAgreement, MarshalAgreement } from "../settings";

describe("Agreements file checker", () => {
    test("Marshal", async () => {
        const r = await fetch(MarshalAgreement, {
            method: "HEAD",
        });
        expect(r.status).toBe(200);
    });
    test("Entrant", async () => {
        const r = await fetch(EntrantAgreement, {
            method: "HEAD",
        });
        expect(r.status).toBe(200);
    });
});
