import type Contact from "../types/contact";

export default class Api {
    private apiUrl = "http://localhost:49001";
    public get = async (id: string) => {
      return await (await fetch(`${this.apiUrl}/${id}`)).json() as Contact;  
    };
    
    getAll = async (query: string) => 
        (await (await fetch(`${this.apiUrl}?${new URLSearchParams({"query": query})}`)).json()) as Contact[];
    
    update =  async (contact: Contact) => {
        await (await fetch(`${this.apiUrl}/${contact.id}`, {
            method: "PUT",
        })).json()
    };

}