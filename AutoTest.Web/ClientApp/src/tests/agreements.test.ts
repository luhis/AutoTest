import { EntrantAgreement, MarshalAgreement } from "../settings";

describe("Agreements file checker", () => {
    test("Marshal", async () => {
        const r = await fetch(MarshalAgreement, {
            method: "HEAD",
        });
        expect(r.ok).toBe(true);
    });
    test("Entrant", async () => {
        const r = await fetch(EntrantAgreement, {
            method: "HEAD",
        });
        expect(r.ok).toBe(true);
    });
    test("Fail", async () => {
        const r = await fetch(
            "https://www.motorsportuk.org/wp-content/uploads/2022/02/XXXX2022-03-01-signing-on-declaration-competitor-table.pdf",
            {
                method: "HEAD",
            }
        );
        expect(r.ok).toBe(false);
    });
});
